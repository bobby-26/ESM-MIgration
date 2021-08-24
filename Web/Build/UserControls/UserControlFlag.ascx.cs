using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlFlag : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int? medicalrequiredyn = null;
    private int _selectedValue = -1;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public int? MedicalRequiredYN
    {
        set
        {
            medicalrequiredyn = value;
        }
        get
        {
            return medicalrequiredyn;
        }
    }

    public DataSet FlagList
    {
        set
        {
            ddlFlag.DataSource = value;
            ddlFlag.DataBind();
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
            ddlFlag.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlFlag.AutoPostBack = true;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlFlag.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlFlag_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedFlag
    {
        get
        {
            return ddlFlag.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlFlag.SelectedIndex = -1;
                ddlFlag.ClearSelection();
                ddlFlag.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlFlag.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlFlag.Items)
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
        ddlFlag.DataSource = PhoenixRegistersFlag.ListFlag(medicalrequiredyn);
        ddlFlag.DataBind();
        foreach (RadComboBoxItem item in ddlFlag.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlFlag_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlFlag.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ddlFlag.Width = value;
        }
        get
        {
            return ddlFlag.Width;
        }
    }
}
