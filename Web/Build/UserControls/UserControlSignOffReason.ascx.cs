using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlSignOffReason : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SignOffReasonList
    {
        set
        {
            ddlSignOffReason.Items.Clear();
            ddlSignOffReason.DataSource = value;
            ddlSignOffReason.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlSignOffReason.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlSignOffReason.AutoPostBack = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlSignOffReason.Enabled = value;
        }
    }
    public string SelectedSignOffReason
    {
        get
        {

            return ddlSignOffReason.SelectedValue;
        }
        set
        {
            ddlSignOffReason.SelectedIndex = -1;
            ddlSignOffReason.Text = "";
            ddlSignOffReason.ClearSelection();
            if (value == string.Empty)
            {               
                ddlSignOffReason.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
                        
            foreach (RadComboBoxItem item in ddlSignOffReason.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    ddlSignOffReason.SelectedValue = value;
                    break;
                }
            }
        }
    }

    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlSignOffReason.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlSignOffReason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSignOffReason.Items)
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
        ddlSignOffReason.DataSource = PhoenixRegistersreasonssignoff.Listreasonssignoff();
        ddlSignOffReason.DataBind();
        foreach (RadComboBoxItem item in ddlSignOffReason.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlSignOffReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlSignOffReason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSignOffReason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ddlSignOffReason.Width = value;
        }
        get
        {
            return ddlSignOffReason.Width;
        }
    }
}
