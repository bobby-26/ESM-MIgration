using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaMail : System.Web.UI.UserControl
{
    int templateCode;
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

    public string TemplateCode
    {
        get
        {
            return templateCode.ToString();
        }
        set
        {
            templateCode = Int32.Parse(value);
        }
    }

    public DataSet MailTemplateList
    {
        set
        {
            ddlMailTemplate.Items.Clear();
            ddlMailTemplate.DataSource = value;
            ddlMailTemplate.DataBind();
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlMailTemplate.AutoPostBack = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlMailTemplate.CssClass = value;
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
            ddlMailTemplate.Enabled = value;
        }
    }

    public AttributeCollection JSAttributes
    {
        get
        {
            return ddlMailTemplate.Attributes;
        }
    }
	public string SelectedName
	{
		get
		{

			return ddlMailTemplate.SelectedItem.Text;
		}
		set
		{
			ddlMailTemplate.SelectedIndex = -1;
			if (value != null && value != "" && value != "0")
				ddlMailTemplate.SelectedItem.Text = value;
		}
	}

    public string SelectedMailTemplate
    {
        get
        {

            return ddlMailTemplate.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlMailTemplate.SelectedIndex = -1;
                ddlMailTemplate.ClearSelection();
                ddlMailTemplate.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlMailTemplate.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlMailTemplate.Items)
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
            ddlMailTemplate.DataSource = PhoenixPreSeaTemplate.ListEmail(null, csvShortName);
            ddlMailTemplate.DataBind();
            foreach (RadComboBoxItem item in ddlMailTemplate.Items)
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


    protected void ddlMailTemplate_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlMailTemplate_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlMailTemplate.Items.Insert(0, new RadComboBoxItem((!string.IsNullOrEmpty(DataBoundItemName) ? DataBoundItemName : "--Select--"), "Dummy"));
    }

    public string ShortNameFilter
    {
        set { csvShortName = value; }
    }
}
