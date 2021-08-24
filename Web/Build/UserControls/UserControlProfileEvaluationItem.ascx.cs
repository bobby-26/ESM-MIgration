using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class UserControlProfileEvaluationItem : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string category = string.Empty;
    string rankcategory = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public void bind()
    {
        ddlProfileEvaluation.DataSource = PhoenixRegistersAppraisalProfileQuestion.ListAppraisalProfileQuestion 
            (General.GetNullableInteger(category) == null? 0 : int.Parse(category),
            General.GetNullableInteger(rankcategory) == null ? 0 : int.Parse(rankcategory)); 

        ddlProfileEvaluation.DataBind();

        foreach (RadComboBoxItem item in ddlProfileEvaluation.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public DataSet EvaluationList
    {
        set
        {
            ddlProfileEvaluation.DataSource = value;
            ddlProfileEvaluation.DataBind();
        }
    }

    public string SelectedEvaluation
    {
        get
        {
            return ddlProfileEvaluation.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlProfileEvaluation.SelectedIndex = -1;
                ddlProfileEvaluation.ClearSelection();
                ddlProfileEvaluation.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlProfileEvaluation.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlProfileEvaluation.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string CssClass
    {
        set
        {
            ddlProfileEvaluation.CssClass = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
           ddlProfileEvaluation.AutoPostBack = value;
        }
    }

    protected void ddlProfileEvaluation_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlProfileEvaluation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlProfileEvaluation_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    public string Category
    {
        set { category = value; }
    }

    public string RankFilter
    {
        set { rankcategory = value; }
    }
}
