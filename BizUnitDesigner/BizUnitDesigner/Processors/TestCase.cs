using System;
using System.Collections.Generic;
using System.Xml;

namespace BizUnitDesigner
{
  public class TestCase
  {
    public string xmlFile="";
    string processingState = "";
    Designer form;
    public Dictionary<string, Step> testSteps = new Dictionary<string, Step>();
    Random ran = new Random();
    public TestCase(Designer form)
    {
      this.form = form;
    }

    public void Save()
    {
      try
      {
        processingState = "Creating root and stage nodes";
        XmlDocument xd = new XmlDocument();
        XmlElement testCaseNode = xd.CreateElement("TestCase");
        XmlAttribute xa = xd.CreateAttribute("testName");
        XmlElement previousTestStepNode=null;
        Step previousTestStep = null;
        XmlComment xc = xd.CreateComment("Test case created using BizUnit Designer");
        xa.Value = new System.IO.FileInfo(xmlFile).Name.Replace(".xml","");
        testCaseNode.SetAttributeNode(xa);

        XmlElement testSetupNode = xd.CreateElement("TestSetup");
        XmlElement testExecutionNode = xd.CreateElement("TestExecution");
        XmlElement testCleanupNode = xd.CreateElement("TestCleanup");
        testCaseNode.AppendChild(testSetupNode);
        testCaseNode.AppendChild(testExecutionNode);
        testCaseNode.AppendChild(testCleanupNode);

        List<String> strItemNames = new List<string>();
        foreach (object obj in form.lstSetup.Items)
          strItemNames.Add(((FormatName)obj).Name);
        foreach (object obj in form.lstExecute.Items)
          strItemNames.Add(((FormatName)obj).Name);
        foreach (object obj in form.lstCleanup.Items)
          strItemNames.Add(((FormatName)obj).Name);

        foreach (string strItemName in strItemNames)
        {
          Step step = testSteps["" + strItemName];
          XmlElement stepNode = null;
          processingState = "Creating test steps";

          if (step.Cat == StepCategory.ContextLoader)
          {
            if (!previousTestStep.StepAttributes.ContainsKey("ContextLoaderStep"))
              throw new Exception("Test step " + previousTestStep.Name.Replace("T:", "") + " cannot host any context loader steps");
            stepNode = xd.CreateElement("ContextLoaderStep");
            previousTestStepNode.AppendChild(stepNode);
          }

          if (step.Cat == StepCategory.ValidationStep)
          {
            if (!previousTestStep.StepAttributes.ContainsKey("ValidationStep"))
              throw new Exception("Test step "+previousTestStep.Name.Replace("T:", "") +" cannot host any validation steps");
            stepNode = xd.CreateElement("ValidationStep");
            previousTestStepNode.AppendChild(stepNode);
          }

          if (step.Cat == StepCategory.TestStep)
          {
            if (step.Name == "*NonGUITestStep")
            {
              XmlDocument xd1 = new XmlDocument();
              xd1.LoadXml(step.StepAttributes["TestCaseXml"].Val);
              xa = xd1.CreateAttribute("NonGUITestStep");
              xa.Value = "true";
              ((XmlElement)xd1.FirstChild).SetAttributeNode(xa);
              stepNode= (XmlElement)xd.ImportNode(xd1.FirstChild,true);
              if (form.lstSetup.Items.Contains(new FormatName(strItemName)))
                testSetupNode.AppendChild(stepNode);
              if (form.lstExecute.Items.Contains(new FormatName(strItemName)))
                testExecutionNode.AppendChild(stepNode);
              if (form.lstCleanup.Items.Contains(new FormatName(strItemName)))
                testCleanupNode.AppendChild(stepNode);
              continue;
            }
            else
            {
              stepNode = xd.CreateElement("TestStep");
              previousTestStepNode = stepNode;
              previousTestStep = step;
              if (form.lstSetup.Items.Contains(new FormatName(strItemName)))
                testSetupNode.AppendChild(stepNode);
              if (form.lstExecute.Items.Contains(new FormatName(strItemName)))
                testExecutionNode.AppendChild(stepNode);
              if (form.lstCleanup.Items.Contains(new FormatName(strItemName)))
                testCleanupNode.AppendChild(stepNode);
            }
          }

          xa = xd.CreateAttribute("assemblyPath");
          xa.Value = "";
          stepNode.SetAttributeNode(xa);
          xa = xd.CreateAttribute("typeName");
          xa.Value = step.Name.Replace("T:", "") + ", " + step.AssemblyName.Remove(step.AssemblyName.Length-4,4);
          stepNode.SetAttributeNode(xa);

          Dictionary<string, StepAttribute>.Enumerator enumerator1 = step.StepAttributes.GetEnumerator();
          while (enumerator1.MoveNext())
          {
            processingState = "Creating step attributes for teststep: " + step.Name.Replace("T:", "");
            if (step.Name == "ValidationStep" || step.Name == "ContextLoaderStep")
              continue;
            if (enumerator1.Current.Value.Val != null && enumerator1.Current.Value.Val != "")
            {
              String[] names = enumerator1.Current.Value.Name.Split('/');
              String[] values = enumerator1.Current.Value.Val.Split(new string[] { ";;" },StringSplitOptions.RemoveEmptyEntries);
              if (!enumerator1.Current.Value.Props.Contains("repeating") && !enumerator1.Current.Value.Props.Contains("more") && values.Length>1)
                throw new Exception("Cannot insert multiple value into attribute: " + enumerator1.Current.Value.Name + " in test step: " + step.Name);
              for (int i = 0; i < values.Length;i++ )
              {
                if (!names[names.Length - 1].Contains("@"))
                {
                  XmlElement stepAttributeNode = xd.CreateElement(names[names.Length - 1]);
                  if (values[i].StartsWith("takeFromCtx="))
                  {
                    xa = xd.CreateAttribute("takeFromCtx");
                    xa.Value = values[i].Replace("takeFromCtx=", "");
                    stepAttributeNode.SetAttributeNode(xa);
                  }
                  else
                  {
                    XmlText xt = xd.CreateTextNode(values[i]);
                    stepAttributeNode.AppendChild(xt);
                  }
                  GetStepAttributeNode(xd, stepNode, enumerator1.Current.Value, 1).AppendChild(stepAttributeNode);
                }
                else
                {
                  xa = xd.CreateAttribute(names[names.Length - 1].Replace("@", ""));
                  xa.Value = values[i];
                  GetStepAttributeNode(xd, stepNode, enumerator1.Current.Value, i + 1).SetAttributeNode(xa);
                }
              }
            }
            else
            {
              if (!enumerator1.Current.Value.Props.Contains("optional"))
                System.Windows.Forms.MessageBox.Show("Attribute: " + enumerator1.Current.Value.Name + " does not have a value. This might cause problems since it is marked as mandatory for test step: " + step.Name,
                  "Required attribute missing", System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Information);
            }
          }
        }

        xd.AppendChild(xc);
        xd.AppendChild(testCaseNode);
        System.IO.File.WriteAllText(xmlFile, XmlFormatter.MakeReadable(xd.InnerXml));
      }
      catch (Exception e)
      {
        System.Windows.Forms.MessageBox.Show("Error occured while: " + processingState + "\r\n\r\n" + "Actual error:\r\n" + e.Message,
            "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
      }
    }

    private XmlElement GetStepAttributeNode(XmlDocument xd, XmlElement stepNode, StepAttribute sa, int number)
    {
      String[] names = sa.Name.Split('/');
      int num = 1;
      XmlElement stepParentNode= stepNode;
      XmlElement stepChildNode;
      for (int i = 0; i <= names.Length - 2; i++)
      {
        if (i == names.Length - 2)
          num=number;
        if (stepParentNode.GetElementsByTagName(names[i]).Count < num)
        {
          stepChildNode = xd.CreateElement(names[i]);
          stepParentNode.AppendChild(stepChildNode);
        }
        else
        {
          stepChildNode = (XmlElement)stepParentNode.GetElementsByTagName(names[i])[num-1];
        }
        stepParentNode = stepChildNode;
      }
      return stepParentNode;
    }

    public void Load()
    {
      testSteps.Clear();
      try
      {
        processingState = "Loading test case";
        XmlDocument xd = new XmlDocument();
        xd.Load(xmlFile);

        processingState = "Checking for test step elements";
        XmlNodeList testStepNodes = xd.GetElementsByTagName("TestStep");
        if (testStepNodes.Count < 1)
          throw new Exception("No test steps found");

        foreach (XmlNode testStepNode in testStepNodes)
        {
          processingState = "Retreiving test steps";
          if (((XmlElement)testStepNode).GetAttribute("NonGUITestStep") == "true")
          {
            Step sn = Step.Copy(form.td.steps["*NonGUITestStep"]);
            string strTempn = sn.Name + "\t\t\t\t\t\t\t\t" + ran.NextDouble().ToString();
            testSteps.Add(strTempn, sn);
            sn.StepAttributes["TestCaseXml"].Val = XmlFormatter.MakeReadable(((XmlElement)testStepNode).OuterXml);
            if (((XmlElement)testStepNode).ParentNode.Name == "TestSetup")
              form.lstSetup.Items.Add(new FormatName(strTempn));
            if (((XmlElement)testStepNode).ParentNode.Name == "TestExecution")
              form.lstExecute.Items.Add(new FormatName(strTempn));
            if (((XmlElement)testStepNode).ParentNode.Name == "TestCleanup")
              form.lstCleanup.Items.Add(new FormatName(strTempn));
            continue;
          }

          Step s = Step.Copy(form.td.steps["T:" + ((XmlElement)testStepNode).GetAttribute("typeName").Split(',')[0]]);
          string strTemp = s.Name + "\t\t\t\t\t\t\t\t" + ran.NextDouble().ToString();
          testSteps.Add(strTemp, s);
          if (((XmlElement)testStepNode).ParentNode.Name == "TestSetup")
            form.lstSetup.Items.Add(new FormatName(strTemp));
          if (((XmlElement)testStepNode).ParentNode.Name == "TestExecution")
            form.lstExecute.Items.Add(new FormatName(strTemp));
          if (((XmlElement)testStepNode).ParentNode.Name == "TestCleanup")
            form.lstCleanup.Items.Add(new FormatName(strTemp));

          processingState = "Retreiving attributes for test step: " + s.Name;
          Dictionary<string, StepAttribute>.Enumerator enumerator = s.StepAttributes.GetEnumerator();
          while(enumerator.MoveNext())
          {
            if (enumerator.Current.Value.Name == "ValidationStep" || enumerator.Current.Value.Name == "ContextLoaderStep")
              continue;
            SetAttributeValue(testStepNode,enumerator.Current.Value);
          }

          XmlNodeList ctxLoaderStepNodes = ((XmlElement)testStepNode).GetElementsByTagName("ContextLoaderStep");
          foreach (XmlNode ctxLoaderStepNode in ctxLoaderStepNodes)
          {
              Step s1 = Step.Copy(form.td.steps["T:" + ((XmlElement)ctxLoaderStepNode).GetAttribute("typeName").Split(',')[0]]);
            processingState = "Retreiving context loader steps for test step: " + s.Name;
            strTemp = ">  " + s1.Name + "\t\t\t\t\t\t\t\t" + ran.NextDouble().ToString();
            testSteps.Add(""+strTemp, s1);
            if (((XmlElement)ctxLoaderStepNode).ParentNode.ParentNode.Name == "TestSetup")
              form.lstSetup.Items.Add(new FormatName(strTemp));
            if (((XmlElement)ctxLoaderStepNode).ParentNode.ParentNode.Name == "TestExecution")
              form.lstExecute.Items.Add(new FormatName(strTemp));
            if (((XmlElement)ctxLoaderStepNode).ParentNode.ParentNode.Name == "TestCleanup")
              form.lstCleanup.Items.Add(new FormatName(strTemp));

            processingState = "Retreiving context loader attributes for test step: " + s1.Name;
            Dictionary<string, StepAttribute>.Enumerator enumerator1 = s1.StepAttributes.GetEnumerator();
            while (enumerator1.MoveNext())
            {
              if (enumerator1.Current.Value.Name == "ValidationStep" || enumerator1.Current.Value.Name == "ContextLoaderStep")
                continue;
              SetAttributeValue(ctxLoaderStepNode, enumerator1.Current.Value);
            }
          }

          XmlNodeList validationStepNodes = ((XmlElement)testStepNode).GetElementsByTagName("ValidationStep");
          foreach (XmlNode validationStepNode in validationStepNodes)
          {
              Step s2 = Step.Copy(form.td.steps["T:" + ((XmlElement)validationStepNode).GetAttribute("typeName").Split(',')[0]]);
            processingState = "Retreiving validation steps for test step: " + s.Name;
            strTemp = ">  " + s2.Name + "\t\t\t\t\t\t\t\t" + ran.NextDouble().ToString();
            testSteps.Add(""+strTemp, s2);
            if (((XmlElement)validationStepNode).ParentNode.ParentNode.Name == "TestSetup")
              form.lstSetup.Items.Add(new FormatName(strTemp));
            if (((XmlElement)validationStepNode).ParentNode.ParentNode.Name == "TestExecution")
              form.lstExecute.Items.Add(new FormatName(strTemp));
            if (((XmlElement)validationStepNode).ParentNode.ParentNode.Name == "TestCleanup")
              form.lstCleanup.Items.Add(new FormatName(strTemp));

            processingState = "Retreiving validation attributes for test step: " + s2.Name;
            Dictionary<string, StepAttribute>.Enumerator enumerator2 = s2.StepAttributes.GetEnumerator();
            while (enumerator2.MoveNext())
            {
              if (enumerator2.Current.Value.Name == "ValidationStep" || enumerator2.Current.Value.Name == "ContextLoaderStep")
                continue;
              SetAttributeValue(validationStepNode, enumerator2.Current.Value);
            }
          }
        }
      }
      catch (Exception e)
      {
        System.Windows.Forms.MessageBox.Show("Error occured while: " + processingState + "\r\n\r\n" + "Actual error:\r\n" + e.Message,
            "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        testSteps.Clear();
        form.lstSetup.Items.Clear();
        form.lstExecute.Items.Clear();
        form.lstCleanup.Items.Clear();
      }
    }

    private void SetAttributeValue(XmlNode stepNode, StepAttribute sa)
    {
      sa.Val = "";
      String[] names = sa.Name.Split('/');
      XmlElement stepParentNode = (XmlElement)stepNode;
      for (int i = 0; i <= names.Length - 2; i++)
      {
        if (stepParentNode.GetElementsByTagName(names[i]).Count > 0)
          stepParentNode = (XmlElement) stepParentNode.GetElementsByTagName(names[i])[0];
      }
      if (names[names.Length - 1].Contains("@"))
      {
        if (names.Length > 1)
        {
          XmlNodeList nl = ((XmlElement)stepParentNode.ParentNode).GetElementsByTagName(names[names.Length - 2]);
          foreach (XmlNode xn in nl)
          {
            sa.Val = sa.Val + ((XmlElement)xn).GetAttribute(names[names.Length - 1].Replace("@", "")) + ";;";
          }
          sa.Val = sa.Val.Trim(';');
        }
        else
          sa.Val = ""+stepParentNode.GetAttribute(names[0].Replace("@",""));
      }      
      else
      {
        XmlNodeList nl= stepParentNode.GetElementsByTagName(names[names.Length - 1]);
        foreach (XmlNode xn in nl)
        {
          if (((XmlElement)xn).HasAttribute("takeFromCtx"))
            sa.Val = sa.Val+"takeFromCtx=" + ((XmlElement)xn).GetAttribute("takeFromCtx") + ";;";
          else
            sa.Val = sa.Val + ((XmlElement)xn).InnerXml.Trim() + ";;";
        }
        sa.Val = sa.Val.Trim(';');
      }
    }
  }
}
