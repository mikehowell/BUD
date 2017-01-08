using System;
using System.Collections.Generic;
using System.Text;

namespace BizUnitDesigner
{
    public class Step
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _desc;

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        private Dictionary<string,StepAttribute> _stepattributes;

        public Dictionary<string,StepAttribute> StepAttributes
        {
            get { return _stepattributes; }
            set { _stepattributes = value; }
        }

        private StepCategory cat;

        public StepCategory Cat
        {
            get { return cat; }
            set { cat = value; }
        }

        private string assemblyName;

        public string AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }
	

        private string _sample;

        public string Sample
        {
          get { return _sample; }
          set { _sample = value; }
        }
	
        public static Step Copy(Step s)
        {
            Step s1 = new Step();
            s1.Name = s.Name;
            s1.Desc = s.Desc;
            s1.Cat = s.cat;
            s1.Sample = s.Sample;
            s1.AssemblyName = s.AssemblyName;
            s1.StepAttributes = new Dictionary<string, StepAttribute>();
            Dictionary<string, StepAttribute>.Enumerator enumerator = s.StepAttributes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                StepAttribute sa = new StepAttribute();
                sa.Desc = enumerator.Current.Value.Desc;
                sa.Name = enumerator.Current.Value.Name;
                sa.Props = enumerator.Current.Value.Props;
                sa.Val = enumerator.Current.Value.Val;
                s1.StepAttributes.Add(sa.Name,sa);
            }
            return s1;
        }
    }

    public enum StepCategory
    {
        TestStep,ValidationStep,ContextLoader
    }
}
