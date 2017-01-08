using System;
using System.Collections.Generic;
using System.Xml;

namespace BizUnitDesigner
{
  public class TestDetails
  {
    string processingState = "";
    public Dictionary<string, Step> steps;

    public void Load()
    {
      steps = new Dictionary<string, Step>();
      string[] strFiles = System.IO.Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Metadata");
      foreach (string xmlFile in strFiles)
      {
        try
        {
          processingState = "Loading the documentation file: " + xmlFile;
          XmlDocument xd = new XmlDocument();
          xd.Load(xmlFile);
          String assemblyName = xd.GetElementsByTagName("assembly")[0].FirstChild.InnerText + ".dll";

          processingState = "Checking for members element: " + xmlFile;
          XmlNodeList membersNode = xd.GetElementsByTagName("members")[0].ChildNodes;
          if (membersNode.Count < 1)
            throw new Exception("No members node found");

          foreach (XmlNode memberNode in membersNode)
          {
            processingState = "Checking for proper member element: " + xmlFile;
            if (!(memberNode is XmlElement))
              continue;
            Step s = new Step();
            s.Name = ((XmlElement)memberNode).GetAttribute("name");
            if (!s.Name.StartsWith("T:"))
              continue;
            if (s.Name.Contains("ContextLoader"))
              s.Cat = StepCategory.ContextLoader;
            else
              if (s.Name.Contains("ValidationStep"))
                s.Cat = StepCategory.ValidationStep;
              else
                s.Cat = StepCategory.TestStep;

            s.AssemblyName = assemblyName;
            s.Desc = ((XmlElement)memberNode).GetElementsByTagName("summary")[0].InnerText.Trim();
            s.StepAttributes = new Dictionary<string, StepAttribute>();
            StepAttribute sa;
            processingState = "Checking for items element inside remarks/list element in member: " + s.Name;
            XmlNodeList itemsNode = null;
            try
            {
              s.Sample = ((XmlElement)((XmlElement)memberNode).GetElementsByTagName("remarks")[0]).GetElementsByTagName("code")[0].InnerXml;
              XmlNode listNode = ((XmlElement)((XmlElement)memberNode).GetElementsByTagName("remarks")[0]).GetElementsByTagName("list")[0];
              itemsNode = ((XmlElement)listNode).GetElementsByTagName("item");
            }
            catch (Exception)
            {
            }
            if (itemsNode == null)
              continue;
            if (itemsNode.Count < 1)
              throw new Exception("No items node found");


            foreach (XmlNode itemNode in itemsNode)
            {
              sa = new StepAttribute();
              processingState = "Getting item details in member: " + s.Name;
              sa.Name = ((XmlElement)itemNode).GetElementsByTagName("term")[0].InnerText;
              sa.Desc = ((XmlElement)itemNode).GetElementsByTagName("description")[0].InnerText;
              sa.Props = "";
              if (((XmlElement)itemNode).GetElementsByTagName("description")[0].InnerText.ToLower().Contains("(repeating)"))
                sa.Props = sa.Props + "repeating,";
              if (((XmlElement)itemNode).GetElementsByTagName("description")[0].InnerText.ToLower().Contains("(one or more)"))
                sa.Props = sa.Props + "one or more,";
              if (((XmlElement)itemNode).GetElementsByTagName("description")[0].InnerText.ToLower().Contains("(optional)"))
                sa.Props = sa.Props + "optional";
              if (sa.Props == "")
                sa.Props = "required";
              else
                sa.Props = sa.Props.Trim(',');
              try
              {
                s.StepAttributes.Add(sa.Name, sa);
              }
              catch (Exception)
              { }
            }
            sa = new StepAttribute();
            sa.Name = "@runConcurrently";
            sa.Desc = "If true, runs the next test step immediately without waiting for this step to complete. Default is false.";
            sa.Props = "optional";
            s.StepAttributes.Add(sa.Name, sa);
            sa = new StepAttribute();
            sa.Name = "@failOnError";
            sa.Desc = "If true, causes the test case to fail if this test step fails. Default is true.";
            sa.Props = "optional";
            s.StepAttributes.Add(sa.Name, sa);
            steps.Add(s.Name, s);
          }
        }
        catch (Exception e)
        {
          System.Windows.Forms.MessageBox.Show("Error occured while:\r\n" + processingState + "\r\n\r\n" + "Actual error:\r\n" + e.Message,
              "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }
      }
      Step s1 = AddNonGUISteps();
      steps.Add(s1.Name, s1);
    }

    private Step AddNonGUISteps()
    {
      Step s1 = new Step();
      s1.Name = "*NonGUITestStep";
      s1.Sample = "";
      s1.Desc = "Use this test step to create your test step using xml by ignoring the GUI.";
      s1.Cat = StepCategory.TestStep;
      s1.StepAttributes = new Dictionary<string, StepAttribute>();
      StepAttribute sa1 = new StepAttribute();
      sa1.Name = "TestCaseXml";
      sa1.Desc = "Test case xml including the testcase node and all context & validation steps";
      sa1.Props = "";
      sa1.Val = "";
      s1.StepAttributes.Add(sa1.Name, sa1);
      return s1;
    }
  }
}
