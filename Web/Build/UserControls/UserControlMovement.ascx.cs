using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlMovement : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string activeyn;
    string payableyn;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {            
            _appenddatabounditems = value;          
        }
    }
    public DataSet MovementList
    {
        set
        {
            ddlMovement.DataSource = value;
            ddlMovement.DataBind();
        }
    }
    public string CssClass
    {
        set
        {
            ddlMovement.CssClass = value;
        }

    }
    public bool AutoPostBack
    {
        set
        {            
            ddlMovement.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlMovement.Enabled = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlMovement_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedMovement
    {
        get
        {
            return ddlMovement.SelectedValue;
        }
        set
        {
            ddlMovement.SelectedIndex = -1;
            ddlMovement.Text = "";
            ddlMovement.ClearSelection();

             if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlMovement.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlMovement.Items)
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
        ddlMovement.DataSource = PhoenixRegisterMovement.ListMovement(General.GetNullableInteger(activeyn), General.GetNullableInteger(payableyn));
        ddlMovement.DataBind();

        foreach (RadComboBoxItem item in ddlMovement.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlMovement_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlMovement.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string ActiveYN
    {
        get
        {
            return ActiveYN.ToString();
        }
        set
        {
            activeyn = value;
        }
    }
    public string PayableYN
    {
        get
        {
            return ActiveYN.ToString();
        }
        set
        {
            payableyn = value;
        }
    }
    public Unit Width
    {
        get
        {
            return ddlMovement.Width;
        }
        set
        {
            ddlMovement.Width = value;
        }
    }
}