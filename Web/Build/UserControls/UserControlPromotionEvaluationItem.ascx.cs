using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlPromotionEvaluationItem : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
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
        ddlPromotionQuestion.DataSource = PhoenixRegistersAppraisalPromotionQuestion.ListAppraisalPromotionQuestion(
            General.GetNullableInteger(rankid) == null ? 0 : int.Parse(rankid)
            );

        ddlPromotionQuestion.DataBind();

        foreach (RadComboBoxItem item in ddlPromotionQuestion.Items)
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
            ddlPromotionQuestion.DataSource = value;
            ddlPromotionQuestion.DataBind();
        }
    }

    public string SelectedEvaluation
    {
        get
        {
            return ddlPromotionQuestion.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPromotionQuestion.SelectedIndex = -1;
                ddlPromotionQuestion.ClearSelection();
                ddlPromotionQuestion.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPromotionQuestion.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPromotionQuestion.Items)
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
            ddlPromotionQuestion.CssClass = value;
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
            ddlPromotionQuestion.AutoPostBack = value;
        }
    }

    protected void ddlPromotionQuestion_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPromotionQuestion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlPromotionQuestion_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    public string RankId
    {
        set { rankid = value; }
    }
}
