using System;
using System.Collections.Generic;
using System.Text;

namespace BizUnitDesigner
{
    public class StepAttribute
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

        private string _props;

        public string Props
        {
            get { return _props; }
            set { _props = value; }
        }

        private string _val;

        public string Val
        {
            get { return _val; }
            set { _val = value; }
        }
	
    }
}
