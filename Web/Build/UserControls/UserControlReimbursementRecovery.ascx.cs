using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlReimbursementRecovery : System.Web.UI.UserControl
{
    int hardtypecode;
    bool sortbyshortname = false;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    string csvShortName = string.Empty;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public string HardTypeCode
    {
        get
        {
            return hardtypecode.ToString();
        }
        set
        {
            hardtypecode = Int32.Parse(value);
        }
    }

    public DataSet HardList
    {
        set
        {
            ddlReimbursementRecovery.Items.Clear();
            ddlReimbursementRecovery.DataSource = value;
            ddlReimbursementRecovery.DataBind();
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlReimbursementRecovery.AutoPostBack = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlReimbursementRecovery.CssClass = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }
    public string DataBoundItemName
    {
        set;
        get;
    }
    public bool Enabled
    {
        set
        {
            ddlReimbursementRecovery.Enabled = value;
        }
    }
    public Unit Width
    {
        set
        {
            ddlReimbursementRecovery.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlReimbursementRecovery.Width;
        }
    }
    public AttributeCollection JSAttributes
    {
        get
        {
            return ddlReimbursementRecovery.Attributes;
        }
    }
    public string SelectedName
    {
        get
        {

            return ddlReimbursementRecovery.SelectedItem.Text;
        }
        set
        {
            ddlReimbursementRecovery.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlReimbursementRecovery.SelectedItem.Text = value;
        }
    }

    public string SelectedHard
    {
        get
        {

            return ddlReimbursementRecovery.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlReimbursementRecovery.SelectedIndex = -1;
                ddlReimbursementRecovery.ClearSelection();
                ddlReimbursementRecovery.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlReimbursementRecovery.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlReimbursementRecovery.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
       
        try
        {
           
            ddlReimbursementRecovery.DataSource = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(hardtypecode, sortbyshortname ? byte.Parse("1") : byte.Parse("0"), 1, csvShortName);
             ddlReimbursementRecovery.DataBind();
            foreach (RadComboBoxItem item in ddlReimbursementRecovery.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        catch { }
    }


    protected void ddlReimbursementRecovery_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlReimbursementRecovery_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlReimbursementRecovery.Items.Insert(0, new RadComboBoxItem((!string.IsNullOrEmpty(DataBoundItemName) ? DataBoundItemName : "--Select--"), "Dummy"));
    }

    public bool SortByShortName
    {

        get
        {
            return sortbyshortname;
        }
        set
        {
            sortbyshortname = value;
        }
    }

    public string ShortNameFilter
    {
        set { csvShortName = value; }
    }

    
}
