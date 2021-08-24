using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControls_UserControlInstallType : System.Web.UI.UserControl
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
    public DataSet ucInstallTypeList
    {
        set
        {
            ucInstallType.DataBind();
            ucInstallType.Items.Clear();
            ucInstallType.DataSource = value;
            ucInstallType.DataBind();
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
            ucInstallType.CssClass = value;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            
                ucInstallType.AutoPostBack = value;
        }
    }
    public string SelectedInstallType
    {
        get
        {

            return ucInstallType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucInstallType.SelectedIndex = -1;
                ucInstallType.ClearSelection();
                ucInstallType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucInstallType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucInstallType.Items)
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
        ucInstallType.DataSource = PhoenixRegistersTypeOfInstallation.TypeOfInstallationList();
        ucInstallType.DataBind();
        foreach (RadComboBoxItem item in ucInstallType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucInstallType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucInstallType.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public Unit Width
    {
        set
        {
            ucInstallType.Width = value;
        }
        get
        {
            return ucInstallType.Width;
        }
    }
    
}
