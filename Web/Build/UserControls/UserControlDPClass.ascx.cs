using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControls_UserControlDPClass : System.Web.UI.UserControl
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
    public DataSet DPClassNameList
    {
        set
        {
            ucDPClassName.DataBind();
            ucDPClassName.Items.Clear();
            ucDPClassName.DataSource = value;
            ucDPClassName.DataBind();
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
            ucDPClassName.CssClass = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucDPClassName.AutoPostBack = true;
        }
    }
    public string SelectedDPClass
    {
        get
        {

            return ucDPClassName.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucDPClassName.SelectedIndex = -1;
                ucDPClassName.ClearSelection();
                ucDPClassName.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucDPClassName.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucDPClassName.Items)
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
        ucDPClassName.DataSource = PhoenixRegistersDPClass.DPClassList();
        ucDPClassName.DataBind();
        foreach (RadComboBoxItem item in ucDPClassName.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucDPClassName_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucDPClassName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ucDPClassName.Width = value;
        }
        get
        {
            return ucDPClassName.Width;
        }
    }

}
