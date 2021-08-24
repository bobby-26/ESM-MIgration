using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlConductEvaluationItem : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string category = string.Empty;
    string rankid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public void bind()
    {
        ddlConductEvaluation.DataSource = PhoenixRegistersAppraisalConductQuestion.ListAppraisalConductQuestion(        
            General.GetNullableInteger(rankid) == null ? 0 : int.Parse(rankid)
            );

        ddlConductEvaluation.DataBind();

        foreach (RadComboBoxItem item in ddlConductEvaluation.Items)
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
            ddlConductEvaluation.DataSource = value;
            ddlConductEvaluation.DataBind();
        }
    }

    public string SelectedEvaluation
    {
        get
        {
            return ddlConductEvaluation.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlConductEvaluation.SelectedIndex = -1;
                ddlConductEvaluation.ClearSelection();
                ddlConductEvaluation.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlConductEvaluation.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlConductEvaluation.Items)
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
            ddlConductEvaluation.CssClass = value;
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
            ddlConductEvaluation.AutoPostBack = value;
        }
    }

    protected void ddlConductEvaluation_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlConductEvaluation.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlConductEvaluation_TextChanged(object sender, EventArgs e)
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
        set { rankid = value; }
    }
}
