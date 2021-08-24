using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewNewApplicantOnboardTraining : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantOnboardTraining.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('dgCrewOnboardTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCrewOnboardTraining.AccessRights = this.ViewState;
        MenuCrewOnboardTraining.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            SetEmployeePrimaryDetails();
        }
        BindData();
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
                Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
                , sortexpression, sortdirection
                , 1
                , dgCrewOnboardTraining.PageSize
                , ref iRowCount
                , ref iTotalPageCount);
            General.ShowExcel("Crew OnBoard", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
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
            Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
            , sortexpression, sortdirection
            , 1
            , dgCrewOnboardTraining.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("dgCrewOnboardTraining", "Crew OnBoard", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            dgCrewOnboardTraining.DataSource = ds;
            dgCrewOnboardTraining.VirtualItemCount = iRowCount;
        }
        else
        {
            dgCrewOnboardTraining.DataSource = "";
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void dgCrewOnboardTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT")) return;
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        DeleteCrewOnboardTraining(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewOnboardTrainingId")).Text));
    //    }
    //    else
    //    {
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    SetPageNavigator();
    //}

    private void DeleteCrewOnboardTraining(Guid onboardtrainingid)
    {
        PhoenixCrewOnboardTraining.DeleteOnboardTraining(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , onboardtrainingid);
    }

    protected void dgCrewOnboardTraining_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                //ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                //db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                //if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                Label l = (Label)e.Row.FindControl("lblDTKey");

                LinkButton lb = (LinkButton)e.Row.FindControl("lnkVessel");
                lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewNewApplicantOnboardTrainingList.aspx?CrewOnboardTrainingId=" + l.Text + "');return false;");

            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
                if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD'); return false;");
            }

            ImageButton attC = (ImageButton)e.Row.FindControl("cmdAtt");
            if (attC != null)
            {
                attC.Visible = SessionUtil.CanAccess(this.ViewState, attC.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    attC.ImageUrl = Session["images"] + "/no-attachment.png";
                attC.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.VESSELACCOUNTS + "'); return false;");
            }

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
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

    protected void dgCrewOnboardTraining_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT")) return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            DeleteCrewOnboardTraining(new Guid(((RadLabel)eeditedItem.FindControl("lblCrewOnboardTrainingId")).Text));
            BindData();
            dgCrewOnboardTraining.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void dgCrewOnboardTraining_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgCrewOnboardTraining.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgCrewOnboardTraining_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel l = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVessel");
            if (lb != null)
                lb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantOnboardTrainingList.aspx?CrewOnboardTrainingId=" + l.Text + "');return false;");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD'); return false;");
            }
        }
    }
}
