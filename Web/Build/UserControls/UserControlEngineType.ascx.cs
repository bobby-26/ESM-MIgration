using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlEngineType : System.Web.UI.UserControl
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

    public DataSet EngineNameList
    {
        set
        {
            ucEngineName.DataBind();
            ucEngineName.Items.Clear();
            ucEngineName.DataSource = value;
            ucEngineName.DataBind();
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
            ucEngineName.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucEngineName.AutoPostBack = true;
        }
    }


    public string SelectedEngineName
    {
        get
        {

            return ucEngineName.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucEngineName.SelectedIndex = -1;
                ucEngineName.ClearSelection();
                ucEngineName.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucEngineName.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucEngineName.Items)
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
        ucEngineName.DataSource = PhoenixRegistersVesselEngine.Listvesselengine();
        ucEngineName.DataBind();
        foreach (RadComboBoxItem item in ucEngineName.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucEngineName_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucEngineName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ucEngineName.Width = value;
        }
        get
        {
            return ucEngineName.Width;
        }
    }
}
