using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlEngineModel : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet ModelNameList
    {
        set
        {
            ucModelName.DataBind();
            ucModelName.Items.Clear();
            ucModelName.DataSource = value;
            ucModelName.DataBind();
        }
    }

    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenddatabounditems = true;
            else
                _appenddatabounditems = false;

        }
    }

    public string CssClass
    {
        set
        {
            ucModelName.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucModelName.AutoPostBack = true;
        }
    }

    public string SelectedModelName
    {
        get
        {
            return ucModelName.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucModelName.SelectedIndex = -1;
                ucModelName.ClearSelection();
                ucModelName.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucModelName.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucModelName.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    private void bind()
    {
        ucModelName.DataSource = PhoenixRegistersVesselEngine.ListVesselModel();
        ucModelName.DataBind();
        foreach (RadComboBoxItem item in ucModelName.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucModelName_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucModelName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
