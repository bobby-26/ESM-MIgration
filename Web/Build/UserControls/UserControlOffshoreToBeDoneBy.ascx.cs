using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class UserControlOffshoreToBeDoneBy : System.Web.UI.UserControl
{
    string typeoftraining = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public string TypeOfTraining
    {
        get
        {
            return typeoftraining;
        }
        set
        {
            typeoftraining = value;
        }
    }
    public DataSet ToBeDoneByList
    {
        set
        {
            ddlToBeDoneBy.DataBind();
            ddlToBeDoneBy.Items.Clear();
            ddlToBeDoneBy.DataSource = value;
            ddlToBeDoneBy.DataBind();
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
            ddlToBeDoneBy.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlToBeDoneBy.AutoPostBack = true;
        }
    }


    public string SelectedToBeDoneBy
    {
        get
        {

            return ddlToBeDoneBy.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlToBeDoneBy.SelectedIndex = -1;
                ddlToBeDoneBy.ClearSelection();
                ddlToBeDoneBy.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlToBeDoneBy.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlToBeDoneBy.Items)
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
                ddlToBeDoneBy.Enabled = true;
            else
                ddlToBeDoneBy.Enabled = false;
        }
    }
    public void bind()
    {
        ddlToBeDoneBy.DataSource = PhoenixCrewOffshoreTrainingNeeds.ListTrainingToBeDoneBy(130, General.GetNullableInteger(typeoftraining));
        ddlToBeDoneBy.DataBind();
        foreach (RadComboBoxItem item in ddlToBeDoneBy.Items)
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
            return ddlToBeDoneBy.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlToBeDoneBy.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlToBeDoneBy.Items)
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

    protected void ddlToBeDoneBy_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlToBeDoneBy_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlToBeDoneBy.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    } 
}