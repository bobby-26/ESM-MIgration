using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlHardExtn : System.Web.UI.UserControl
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
            ddlHard.Items.Clear();
            ddlHard.DataSource = value;
            ddlHard.DataBind();
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlHard.AutoPostBack = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlHard.CssClass = value;
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
            ddlHard.Enabled = value;
        }
    }
    public Unit Width
    {
        set
        {
            ddlHard.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlHard.Width;
        }
    }
    public AttributeCollection JSAttributes
    {
        get
        {
            return ddlHard.Attributes;
        }
    }
    public string SelectedName
    {
        get
        {

            return ddlHard.SelectedItem.Text;
        }
        set
        {
            ddlHard.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlHard.SelectedItem.Text = value;
        }
    }

    public string SelectedHard
    {
        get
        {

            return ddlHard.SelectedValue;
        }
        set
        {
            ddlHard.SelectedIndex = -1;
            ddlHard.Text = "";
            ddlHard.ClearSelection();
            if (value == string.Empty)
            {

                ddlHard.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            //ddlHard.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHard.Items)
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
            ddlHard.DataSource = PhoenixRegistersHardExtn.ListHardExtn(hardtypecode, sortbyshortname ? byte.Parse("1") : byte.Parse("0"), csvShortName, Pool);
            ddlHard.DataBind();
            foreach (RadComboBoxItem item in ddlHard.Items)
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
    protected void ddlHard_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlHard_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlHard.Items.Insert(0, new RadComboBoxItem((!string.IsNullOrEmpty(DataBoundItemName) ? DataBoundItemName : "--Select--"), "Dummy"));
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

    public int? Pool
    {
        get;
        set;
    }
}