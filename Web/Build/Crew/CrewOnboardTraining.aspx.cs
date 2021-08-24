using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewOnboardTraining : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmenu = new PhoenixToolbar();
        CrewCourse.AccessRights = this.ViewState;
        CrewCourse.MenuList = toolbarmenu.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewOnboardTraining.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgCrewOnboardTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCrewOnboardTraining.AccessRights = this.ViewState;
        MenuCrewOnboardTraining.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            dgCrewOnboardTraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            SetEmployeePrimaryDetails();
        }

    }

    protected void CrewOnboardTraining_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDSUBJECT", "FLDFROMDATE", "FLDTODATE", "FLDTRAINERNAME", "FLDREMARKS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
            string[] alCaptions = { "Vessel", "Training Name", "From Date", "To Date", "Trainer By", "Remarks", "Updated By", "Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewOnboardTraining.OnboardTrainingSearch(
                Int32.Parse(Filter.CurrentCrewSelection.ToString())
                , sortexpression, sortdirection
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);
            General.ShowExcel("Onboard Training", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        dgCrewOnboardTraining.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDSUBJECT", "FLDFROMDATE", "FLDTODATE", "FLDTRAINERNAME", "FLDREMARKS", "FLDUPDATEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Vessel", "Training Name", "From Date", "To Date", "Trainer By", "Remarks", "Updated By", "Date" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOnboardTraining.OnboardTrainingSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , dgCrewOnboardTraining.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("dgCrewOnboardTraining", "Onboard Training", alCaptions, alColumns, ds);

        dgCrewOnboardTraining.DataSource = ds;
        dgCrewOnboardTraining.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void dgCrewOnboardTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgCrewOnboardTraining.CurrentPageIndex + 1;
        BindData();
    }

    protected void dgCrewOnboardTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void dgCrewOnboardTraining_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        Guid onboardtrainingid = new Guid(((RadLabel)e.Item.FindControl("lblCrewOnboardTrainingId")).Text);

        PhoenixCrewOnboardTraining.DeleteOnboardTraining(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , onboardtrainingid);

        BindData();
        dgCrewOnboardTraining.Rebind();
    }




    protected void dgCrewOnboardTraining_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void dgCrewOnboardTraining_EditCommand(object sender, GridCommandEventArgs e)
    {

    }


    protected void dgCrewOnboardTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkTrainingName");
            RadLabel l = (RadLabel)e.Item.FindControl("lblDTKey");
            if (cme != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
                lb.Enabled = SessionUtil.CanAccess(this.ViewState, cme.CommandName);


                cme.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOnboardTrainingList.aspx?CrewOnboardTrainingId=" + l.Text + "');return false;");
            }

            if (lb != null)
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOnboardTrainingList.aspx?CrewOnboardTrainingId=" + l.Text + "');return false;");
            }



            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());

            if (cmdAttachment != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName)) cmdAttachment.Visible = false;

                if (n == 0)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdAttachment.Controls.Add(html);
                }

                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.ToString() + "&mod="
                                  + PhoenixModule.VESSELACCOUNTS + "');return true;");
            }


            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            if (lbtn != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }
            
        }
    }
  
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        dgCrewOnboardTraining.Rebind();
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }


}
