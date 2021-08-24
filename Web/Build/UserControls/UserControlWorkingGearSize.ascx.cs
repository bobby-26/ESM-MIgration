using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class UserControls_UserControlWorkingGearSize : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private Guid? itemid = null;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SizeList
    {
        set
        {
            ddlWorkingGearSize.DataBind();
            ddlWorkingGearSize.Items.Clear();
            ddlWorkingGearSize.DataSource = value;
            ddlWorkingGearSize.DataBind();
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
            ddlWorkingGearSize.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlWorkingGearSize.AutoPostBack = true;
        }
    }

    public string SelectedSize
    {
        get
        {
            return ddlWorkingGearSize.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlWorkingGearSize.SelectedIndex = -1;
                ddlWorkingGearSize.ClearSelection();
                ddlWorkingGearSize.Text = string.Empty; 
                return;
            }
            _selectedValue = value;
            ddlWorkingGearSize.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlWorkingGearSize.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string Enabled
    {
        set
        {

            if (value.ToUpper().Equals("TRUE"))
                ddlWorkingGearSize.Enabled = true;
            else
                ddlWorkingGearSize.Enabled = false;
        }
    }

    public Guid? ItemId
    {
        get
        {
            return ItemId;
        }
        set
        {
            ItemId = value;
        }
    }

  
    public void bind()
    {
        ddlWorkingGearSize.DataSource = PhoenixWorkingGearSize.ListSize(
            itemid);

        ddlWorkingGearSize.DataBind();
        foreach (RadComboBoxItem item in ddlWorkingGearSize.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlWorkingGearSize.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlWorkingGearSize.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlWorkingGearSize.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlWorkingGearSize_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlWorkingGearSize_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlWorkingGearSize.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
