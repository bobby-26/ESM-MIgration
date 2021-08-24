using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionDashBoardDeficiencyOfficeListFilter : PhoenixBasePage
{
   protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuDefeciencyFilter.AccessRights = this.ViewState;
        MenuDefeciencyFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            BindInspection();

        }
    }

    protected void MenuDefeciencyFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucCompany", ucCompany.SelectedCompany);
            criteria.Add("uccategory", ucInspectionCategory.SelectedHard);
            criteria.Add("ucinspection", ddlInspection.SelectedValue);
            criteria.Add("ucchapter", ucChapter.SelectedChapter);
            criteria.Add("ucdefeciencytype", ddlNCType.SelectedValue == "0" ? null : ddlNCType.SelectedValue);
            criteria.Add("ucdefeciencycategory", ucNonConformanceCategory.SelectedQuick);
            criteria.Add("ucsource", ddlSource.SelectedValue == "0" ? null : ddlSource.SelectedValue);
            criteria.Add("ucsourceno", txtSourceRefNo.Text.Trim());
            criteria.Add("ucreferenceno", txtRefNo.Text.Trim());

            InspectionFilter.CurrentDeficiencyDashboardFilter = criteria;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
    protected void ucInspection_Changed(object sender, EventArgs e)
    {
        ucChapter.InspectionId = ddlInspection.SelectedValue;
        ucChapter.ChapterList = PhoenixInspectionChapter.ListInspectionChapter(General.GetNullableInteger(null),
            General.GetNullableInteger(ucInspectionCategory.SelectedHard),
            General.GetNullableGuid(ddlInspection.SelectedValue));
    }
    protected void ucInspectionType_Changed(object sender, EventArgs e)
    {
        BindInspection();
    }

    protected void ucInspectionCategory_TextChangedEvent(object sender, EventArgs e)
    {
        BindInspection();
    }
    protected void BindInspection()
    {
        ddlInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(null)
                                       , General.GetNullableInteger(ucInspectionCategory.SelectedHard)
                                       , null
                                       , 1
                                       , General.GetNullableInteger(ucCompany.SelectedCompany));
        ddlInspection.DataTextField = "FLDSHORTCODE";
        ddlInspection.DataValueField = "FLDINSPECTIONID";
        ddlInspection.DataBind();
        ddlInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ucCompany_TextChangedEvent(object sender, EventArgs e)
    {
        BindInspection();
    }
}