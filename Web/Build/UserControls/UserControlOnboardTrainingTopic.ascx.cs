using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlOnboardTrainingTopic : System.Web.UI.UserControl
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

    public DataSet OnboardTrainingTopicList
    {
        set
        {          
            ddlOnboardTrainingTopic.Items.Clear();
            ddlOnboardTrainingTopic.DataSource = value;
            ddlOnboardTrainingTopic.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlOnboardTrainingTopic.CssClass = value;
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
                ddlOnboardTrainingTopic.AutoPostBack = true;
        }
    }

    public string SelectedOnboardTrainingTopic
    {
        get
        {
            return ddlOnboardTrainingTopic.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOnboardTrainingTopic.SelectedIndex = -1;
                ddlOnboardTrainingTopic.ClearSelection();
                ddlOnboardTrainingTopic.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlOnboardTrainingTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOnboardTrainingTopic.Items)
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
            return (int.Parse(ddlOnboardTrainingTopic.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlOnboardTrainingTopic.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOnboardTrainingTopic.Items)
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
        ddlOnboardTrainingTopic.DataSource = PhoenixRegistersMiscellaneousOnBoardTrainingTopic.ListMiscellaneousOnBoardTrainingTopic();
        ddlOnboardTrainingTopic.DataBind();
        foreach (RadComboBoxItem item in ddlOnboardTrainingTopic.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlOnboardTrainingTopic_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOnboardTrainingTopic.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string Width
    {
        get
        {
            return ddlOnboardTrainingTopic.Width.ToString();
        }
        set
        {
            ddlOnboardTrainingTopic.Width = Unit.Parse(value);
        }
    }
    public bool Enabled
    {
        get
        {
            return ddlOnboardTrainingTopic.Enabled;
        }
        set
        {
            ddlOnboardTrainingTopic.Enabled = value;
        }
    }

}
