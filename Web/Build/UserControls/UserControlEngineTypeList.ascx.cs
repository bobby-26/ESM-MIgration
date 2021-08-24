using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class UserControlEngineTypeList : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstEngineType.DataSource = PhoenixRegistersVesselEngine.Listvesselengine();
            lstEngineType.DataBind();
        }
    }

    public DataSet EngineTypeList
    {
        set
        {            
            lstEngineType.Items.Clear();
            lstEngineType.DataSource = value;
            lstEngineType.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {           
            _appenddatabounditems = value;
        }
    }


    public string CssClass
    {
        set
        {
            chkboxlist.Attributes["class"] = value;
            lstEngineType.CssClass = value;
        }
    }

    public Unit Width
    {
        set
        {
            lstEngineType.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstEngineType.Width;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            lstEngineType.AutoPostBack = value;
        }
    }


    public string SelectedEngineTypeList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstEngineType.Items)
            {
                if (item.Checked)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstEngineType.Items)
            {
                if (item.Checked)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            value = strlist.ToString();
        }
    }

    protected void lstEngineType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            lstEngineType.Items.Insert(0, new RadListBoxItem("--Select--", ""));
    } 
}
