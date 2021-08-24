using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlBriefingTopic : System.Web.UI.UserControl
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

    public DataSet BriefingTopicList
    {
        set
        {
            ddlBriefingTopic.Items.Clear();
            ddlBriefingTopic.DataSource = value;
            ddlBriefingTopic.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlBriefingTopic.CssClass = value;
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

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBriefingTopic.AutoPostBack = true;
        }
    }

    public string SelectedBriefingTopic
    {
        get
        {
            return ddlBriefingTopic.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBriefingTopic.SelectedIndex = -1;
                ddlBriefingTopic.ClearSelection();
                ddlBriefingTopic.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlBriefingTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBriefingTopic.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlBriefingTopic.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlBriefingTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBriefingTopic.Items)
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
        ddlBriefingTopic.DataSource = PhoenixRegistersMiscellaneousBriefingTopic.ListMiscellaneousBriefingTopic();
        ddlBriefingTopic.DataBind();
        foreach (RadComboBoxItem item in ddlBriefingTopic.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlBriefingTopic_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBriefingTopic.Items.Insert(0, new RadComboBoxItem("--Select--","Dummy"));
    } 
}
