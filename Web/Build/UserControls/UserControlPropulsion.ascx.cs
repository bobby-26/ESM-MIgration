using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlPropulsion : System.Web.UI.UserControl
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
    public DataSet PropulsionNameList
    {
        set
        {
            ucPropulsionName.DataBind();
            ucPropulsionName.Items.Clear();
            ucPropulsionName.DataSource = value;
            ucPropulsionName.DataBind();
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
            ucPropulsionName.CssClass = value;
        }
    }


    public bool AutoPostBack
    {
        set
        {         
            ucPropulsionName.AutoPostBack = value;
        }
    }
    public string SelectedPropulsion
    {
        get
        {

            return ucPropulsionName.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucPropulsionName.SelectedIndex = -1;
                ucPropulsionName.ClearSelection();
                ucPropulsionName.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucPropulsionName.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucPropulsionName.Items)
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
        ucPropulsionName.DataSource = PhoenixRegistersVesselEngine.PropulsionList();
        ucPropulsionName.DataBind();
        foreach (RadComboBoxItem item in ucPropulsionName.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucPropulsionName_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucPropulsionName.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public Unit Width
    {
        set
        {
            ucPropulsionName.Width = value;
        }
        get
        {
            return ucPropulsionName.Width;
        }
    }
}
