using System;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlTradingArea_: System.Web.UI.UserControl
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
    public DataSet TradingAreaList
    {
        set
        {
            UserControlTradingArea.DataBind();
            UserControlTradingArea.Items.Clear();
            UserControlTradingArea.DataSource = value;
            UserControlTradingArea.DataBind();
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
            UserControlTradingArea.CssClass = value;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            
                UserControlTradingArea.AutoPostBack = value;
        }
    }
    public string SelectedTradingArea
    {
        get
        {

            return UserControlTradingArea.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                UserControlTradingArea.SelectedIndex = -1;
                UserControlTradingArea.ClearSelection();
                UserControlTradingArea.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            UserControlTradingArea.SelectedIndex = -1;
            foreach (RadComboBoxItem item in UserControlTradingArea.Items)
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
        UserControlTradingArea.DataSource = PhoenixRegistersTradingArea.TradingAreaList();
        UserControlTradingArea.DataBind();
        foreach (RadComboBoxItem item in UserControlTradingArea.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucTradingArea_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            UserControlTradingArea.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public Unit Width
    {
        set
        {
            UserControlTradingArea.Width = value;
        }
        get
        {
            return UserControlTradingArea.Width;
        }
    }
}
