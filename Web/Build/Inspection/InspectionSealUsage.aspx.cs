using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionSealUsage : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvSealUsage.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvSealUsage.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }

    //    base.Render(writer);    
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSealUsage.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSealUsage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Inspection/InspectionSealUsageFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSealUsage.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuSeal.AccessRights = this.ViewState;
            MenuSeal.MenuList = toolbar.Show();
            // MenuSeal.SetTrigger(pnlSealEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["REMARKSID"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSealUsage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["FILTER"] != null)
        {
            if (Session["FILTER"].ToString() == "1")
            {
                ViewState["PAGENUMBER"] = 1;

                //BindData();
                gvSealUsage.Rebind();
                //SetPageNavigator();

                Session["FILTER"] = "0";
            }
        }
        if (Session["POPUPSAVE"] != null)
        {
            if (Session["POPUPSAVE"].ToString() == "1")
            {
               // BindData();
                gvSealUsage.Rebind();
                //SetPageNavigator();

                Session["POPUPSAVE"] = "0";
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds;
        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTNAME", "FLDSEALNO", "FLDSEALTYPENAME", "FLDPERSONAFFIXINGSEAL", "FLDDATEAFFIXED", "FLDREASONNAME", "FLDUSAGEREMARKSNAME", "FLDREMARKSNAME" };
        string[] alCaptions = { "Location", "Seal Point", "Seal Number", "Seal Type", "Seal Affixed by", "Date Affixed", "Reason", "Remarks", "Remarks Inspection/Incident/Others" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentSealUsageFilter;

        ds = PhoenixInspectionSealUsage.SealUsageSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , nvc != null ? General.GetNullableInteger(nvc["sealtype"].ToString()) : null
            , nvc != null ? General.GetNullableInteger(nvc["location"].ToString()) : null
            , nvc != null ? General.GetNullableString(nvc["sealno"]) : null
            , nvc != null ? General.GetNullableString(nvc["sealaffixedby"]) : null
            , nvc != null ? General.GetNullableDateTime(nvc["affixedfrom"]) : null
            , nvc != null ? General.GetNullableDateTime(nvc["affixedto"]) : null
            , nvc != null ? General.GetNullableInteger(nvc["reason"]) : null
            , sortexpression, sortdirection,
           int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvSealUsage.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            null);

        General.SetPrintOptions("gvSealUsage", "Seal Usage", alCaptions, alColumns, ds);

        gvSealUsage.DataSource = ds;
        gvSealUsage.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void MenuSeal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentSealUsageFilter = null;
                //BindData();
                gvSealUsage.Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  


    private bool IsValidSealUsage(string locationid, string sealid, string sealaffixedby, string dateaffixed, string reason, string reasontext, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(locationid) == null || locationid == "0")
            ucError.ErrorMessage = "Location is required.";

        if (General.GetNullableGuid(sealid) == null)
            ucError.ErrorMessage = "Seal Number is required.";

        if (General.GetNullableInteger(sealaffixedby) == null)
            ucError.ErrorMessage = "Seal Affixed by is required.";

        if (General.GetNullableDateTime(dateaffixed) == null)
            ucError.ErrorMessage = "Date Affixed is required.";
        else if (General.GetNullableDateTime(dateaffixed) > DateTime.Today)
            ucError.ErrorMessage = "Date Affixed can not be the future date.";

        if (General.GetNullableInteger(reason) == null)
            ucError.ErrorMessage = "Reason is required.";

        if (General.GetNullableInteger(reason) != null)
        {
            if (reasontext.ToUpper() == "REPLACEMENT")
            {
                if (General.GetNullableInteger(remarks) == null)
                    ucError.ErrorMessage = "Replacement reason is required.";
            }

        }

        return (!ucError.IsError);
    }



    //protected void gvSealUsage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 || General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) == null)
    //        {
    //            ucError.ErrorMessage = "Please switch to vessel.";
    //            ucError.Visible = true;
    //            return;
    //        }

    //        if (!IsValidSealUsage(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLocationidEdit")).Text
    //                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealidEdit")).Text
    //                , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealAffixedbyIdEdit")).Text
    //                , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtAffixedDateEdit")).Text
    //                , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucReasonEdit")).SelectedQuick
    //                , ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucReasonEdit")).SelectedText
    //                , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlRemarksEdit")).SelectedValue
    //                ))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        NameValueCollection nvcRem = new NameValueCollection();
    //        nvcRem = null;
    //        if (Filter.CurrentSealUsageRemarks != null)
    //        {
    //            nvcRem = Filter.CurrentSealUsageRemarks;
    //        }
    //        PhoenixInspectionSealUsage.UpdateSealUsage(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSealUsageIdEdit")).Text)
    //                , new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealidEdit")).Text)
    //                , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealAffixedbyIdEdit")).Text)
    //                , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtAffixedDateEdit")).Text)
    //                , int.Parse(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucReasonEdit")).SelectedQuick)
    //                , General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlRemarksEdit")).SelectedValue)
    //                , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiInspection"].ToString()) : null
    //                , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiIncident"].ToString()) : null
    //                , nvcRem != null ? General.GetNullableString(nvcRem["txtRemarks"].ToString()) : null);
    //        Filter.CurrentSealUsageRemarks = null;

    //        //PhoenixInspectionSealUsage.InsertSealUsageHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID
    //        //        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLocationidEdit")).Text)
    //        //        , new Guid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealidEdit")).Text)
    //        //        , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSealAffixedbyIdEdit")).Text)
    //        //        , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtAffixedDateEdit")).Text)
    //        //        , int.Parse(((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucReasonEdit")).SelectedQuick)
    //        //        , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSealUsageIdEdit")).Text));

    //        _gridView.EditIndex = -1;
    //        BindData();
    //       // SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvSealUsage_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //    if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //    ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //    if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //    ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
    //    if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

    //    ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //    if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //    ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //    if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

    //    ImageButton imgRem = (ImageButton)e.Row.FindControl("cmdViewChangeRemarks");
    //    if (imgRem != null) imgRem.Visible = SessionUtil.CanAccess(this.ViewState, imgRem.CommandName);

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        ImageButton btnShowSealNumberEdit = (ImageButton)e.Row.FindControl("btnShowSealNumberEdit");
    //        if (btnShowSealNumberEdit != null)
    //        {
    //            string status = "";
    //            status = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");
    //           // btnShowSealNumberEdit.Attributes.Add("onclick", "return showPickList('spnPickListSealNumberedit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSealNumber.aspx?iframignore=true&status=" + status + "', true); ");
    //            btnShowSealNumberEdit.Attributes.Add("onclick", "return showPickList('spnPickListSealNumberedit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSealNumber.aspx?iframignore=true&status=" + status + "', true); ");
    //        }
    //        TextBox txtSealNumberEdit = (TextBox)e.Row.FindControl("txtSealNumberEdit");
    //        if (txtSealNumberEdit != null)
    //            txtSealNumberEdit.Text = drv["FLDSEALNO"].ToString();
    //        TextBox txtSealidEdit = (TextBox)e.Row.FindControl("txtSealidEdit");
    //        if (txtSealidEdit != null)
    //            txtSealidEdit.Text = drv["FLDSEALID"].ToString();

    //        ImageButton btnSealAffixedbyEdit = (ImageButton)e.Row.FindControl("btnSealAffixedbyEdit");
    //        if (btnSealAffixedbyEdit != null)
    //        {
    //            btnSealAffixedbyEdit.Attributes.Add("onclick", "return showPickList('spnSealAffixedByEdit', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
    //                + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
    //        }
    //        TextBox txtSealAffixedbyNameEdit = (TextBox)e.Row.FindControl("txtSealAffixedbyNameEdit");
    //        if (txtSealAffixedbyNameEdit != null) txtSealAffixedbyNameEdit.Text = drv["FLDPERSONAFFIXINGSEAL"].ToString();
    //        TextBox txtSealAffixedbyRankEdit = (TextBox)e.Row.FindControl("txtSealAffixedbyRankEdit");
    //        if (txtSealAffixedbyRankEdit != null) txtSealAffixedbyRankEdit.Text = drv["FLDRANKNAME"].ToString();
    //        TextBox txtSealAffixedbyIdEdit = (TextBox)e.Row.FindControl("txtSealAffixedbyIdEdit");
    //        if (txtSealAffixedbyIdEdit != null) txtSealAffixedbyIdEdit.Text = drv["FLDEMPLOYEEID"].ToString();

    //        UserControlQuick ucReasonEdit = (UserControlQuick)e.Row.FindControl("ucReasonEdit");            
    //        DropDownList ddlRemarks = (DropDownList)e.Row.FindControl("ddlRemarksEdit");
    //        if (ddlRemarks != null)
    //        {
    //            ddlRemarks.DataSource = PhoenixInspectionSealUsage.ListSealUsageRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , null
    //                , null);                
    //            ddlRemarks.DataTextField = "FLDUSAGEREMARKSNAME";
    //            ddlRemarks.DataValueField = "FLDSEALUSAGEREMARKSID";
    //            ddlRemarks.DataBind();
    //            ddlRemarks.Items.Insert(0,new ListItem("--Select--","Dummy"));
    //        }
    //        if (ucReasonEdit != null && ddlRemarks != null)
    //        {
    //            ucReasonEdit.bind();
    //            ucReasonEdit.SelectedQuick = drv["FLDREASON"].ToString();

    //            ddlRemarks.SelectedValue = drv["FLDUSAGEREMARKSID"].ToString();
    //            if (ucReasonEdit.SelectedText.ToUpper() == "REPLACEMENT")
    //            {
    //                ddlRemarks.CssClass = "input_mandatory";
    //            }
    //            else
    //            {
    //                ddlRemarks.CssClass = "input";
    //            }
    //            if (drv["FLDUSAGEREMARKSID"].ToString() == "4" || drv["FLDUSAGEREMARKSID"].ToString() == "5" || drv["FLDUSAGEREMARKSID"].ToString() == "6")
    //            {
    //                NameValueCollection nvc = new NameValueCollection();
    //                if (Filter.CurrentSealUsageRemarks == null)
    //                {
    //                    nvc.Add("sealusageid", drv["FLDSEALUSAGEID"].ToString());
    //                    nvc.Add("ucMultiInspection", drv["FLDUSAGEINSPECTION"].ToString());
    //                    nvc.Add("ucMultiIncident", drv["FLDUSAGEINCIDENT"].ToString());
    //                    nvc.Add("txtRemarks", drv["FLDUSAGEREMARKS"].ToString());
    //                    Filter.CurrentSealUsageRemarks = nvc;
    //                }
    //            }
    //        }
    //        ImageButton history = (ImageButton)e.Row.FindControl("cmdHistory");
    //        Label lblSealUsageId = (Label)e.Row.FindControl("lblSealUsageId");
    //        Label lblRemarks = (Label)e.Row.FindControl("lblRemarks");
    //        if (history != null && lblSealUsageId != null)
    //            history.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealUsageHistory.aspx?SEALUSAGEID=" + lblSealUsageId.Text + "',null)");
    //        Label lblSealUsageIdEdit = (Label)e.Row.FindControl("lblSealUsageIdEdit");

    //        if (lblRemarks != null)
    //        {
    //            if (General.GetNullableString(lblRemarks.Text) != null)
    //            {
    //                UserControlToolTip ucToolTipRemarks = (UserControlToolTip)e.Row.FindControl("ucToolTipUsageRemarks");
    //                lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipRemarks.ToolTip + "', 'visible');");
    //                lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipRemarks.ToolTip + "', 'hidden');");
    //            }
    //        }
    //        if (imgRem != null)
    //        {
    //            if (drv["FLDUSAGEREMARKSID"].ToString() == "4" || drv["FLDUSAGEREMARKSID"].ToString() == "5" || drv["FLDUSAGEREMARKSID"].ToString() == "6")
    //            {
    //                imgRem.Visible = true;
    //                imgRem.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealUsageRemarks.aspx?type=2&sealusageid=" + drv["FLDSEALUSAGEID"].ToString() + "&remarksid=" + drv["FLDUSAGEREMARKSID"].ToString() + "','xlarge')");
    //            }
    //            else
    //            {
    //                imgRem.Visible = false;
    //            }
    //        }

    //        DataRowView dv = (DataRowView)e.Row.DataItem;
    //        Image imgFlag = e.Row.FindControl("imgFlag") as Image;
    //        if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("3"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
    //            imgFlag.ToolTip = "Replacement Overdue";
    //        }
    //        else if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("2"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
    //            imgFlag.ToolTip = "Replacement Due within 30 days";
    //        }
    //        else if (imgFlag != null && dv["FLDREPLACEMENTDUE"].ToString().Equals("1"))
    //        {
    //            imgFlag.Visible = true;
    //            imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
    //            imgFlag.ToolTip = "Replacement Due within 60 days";
    //        }
    //        else
    //        {
    //            if (imgFlag != null) imgFlag.Visible = false;
    //        }

    //        RadDropDownList ddlLocationedit = (RadDropDownList)e.Row.FindControl("ddlLocationedit");

    //        if (ddlLocationedit != null)
    //        {
    //            ddlLocationedit.DataSource = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //            ddlLocationedit.DataTextField = "FLDLOCATIONNAME";
    //            ddlLocationedit.DataValueField = "FLDLOCATIONID";
    //            ddlLocationedit.DataBind();
    //            ddlLocationedit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //        }

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        DropDownList ddlLocationAdd = (DropDownList)e.Row.FindControl("ddlLocationAdd");

    //        if (ddlLocationAdd != null)
    //        {
    //            ddlLocationAdd.DataSource = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //            ddlLocationAdd.DataTextField = "FLDLOCATIONNAME";
    //            ddlLocationAdd.DataValueField = "FLDLOCATIONID";
    //            ddlLocationAdd.DataBind();
    //            ddlLocationAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //        }
    //        ImageButton btnShowSealNumberAdd = (ImageButton)e.Row.FindControl("btnShowSealNumberAdd");
    //        if (btnShowSealNumberAdd != null)
    //        {
    //            string status = "";
    //            status = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");
    //            btnShowSealNumberAdd.Attributes.Add("onclick", "return showPickList('spnPickListSealNumberAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSealNumber.aspx?iframignore=true&status=" + status + "', true); ");
    //        }

    //        ImageButton btnSealAffixedbyAdd = (ImageButton)e.Row.FindControl("btnSealAffixedbyAdd");
    //        if (btnSealAffixedbyAdd != null)
    //        {
    //            btnSealAffixedbyAdd.Attributes.Add("onclick", "return showPickList('spnSealAffixedByAdd', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
    //                + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
    //        }

    //        UserControlQuick ucReasonAdd = (UserControlQuick)e.Row.FindControl("ucReasonAdd");
    //        if (ucReasonAdd != null)
    //            ucReasonAdd.bind();

    //        UserControlDate txtAffixedDateAdd = (UserControlDate)e.Row.FindControl("txtAffixedDateAdd");
    //        if (txtAffixedDateAdd != null)
    //        {
    //            txtAffixedDateAdd.Text = DateTime.Today.ToString();
    //        }

    //        DropDownList ddlRemarks = (DropDownList)e.Row.FindControl("ddlRemarksAdd");
    //        if (ddlRemarks != null)
    //        {
    //            ddlRemarks.DataSource = PhoenixInspectionSealUsage.ListSealUsageRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , null
    //                , null);
    //            ddlRemarks.DataTextField = "FLDUSAGEREMARKSNAME";
    //            ddlRemarks.DataValueField = "FLDSEALUSAGEREMARKSID";
    //            ddlRemarks.DataBind();
    //            ddlRemarks.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //        }

    //        if (ucReasonAdd != null && ddlRemarks != null)
    //        {

    //            if (ucReasonAdd.SelectedText.ToUpper() == "REPLACEMENT")
    //            {
    //                ddlRemarks.CssClass = "input_mandatory";
    //            }
    //            else
    //            {
    //                ddlRemarks.CssClass = "input";
    //            }
    //        }
    //    }
    //}


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDLOCATIONNAME", "FLDSEALPOINTNAME", "FLDSEALNO", "FLDSEALTYPENAME", "FLDPERSONAFFIXINGSEAL", "FLDDATEAFFIXED", "FLDREASONNAME", "FLDUSAGEREMARKSNAME", "FLDREMARKSNAME" };
        string[] alCaptions = { "Location", "Seal Point", "Seal Number", "Seal Type", "Seal Affixed by", "Date Affixed", "Reason", "Remarks", "Remarks Inspection/Incident/Others" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentSealUsageFilter;

        ds = PhoenixInspectionSealUsage.SealUsageSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
            , nvc != null ? General.GetNullableInteger(nvc["sealtype"].ToString()) : null
            , nvc != null ? General.GetNullableInteger(nvc["location"].ToString()) : null
            , nvc != null ? General.GetNullableString(nvc["sealno"]) : null
            , nvc != null ? General.GetNullableString(nvc["sealaffixedby"]) : null
            , nvc != null ? General.GetNullableDateTime(nvc["affixedfrom"]) : null
            , nvc != null ? General.GetNullableDateTime(nvc["affixedto"]) : null
            , nvc != null ? General.GetNullableInteger(nvc["reason"]) : null
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            null);

        Response.AddHeader("Content-Disposition", "attachment; filename=SealUsage.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Seal Usage</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }



    protected void ucReasonEdit_Changed(object sender, EventArgs e)
    {
        UserControlQuick ucQuick = (UserControlQuick)sender;
        GridDataItem gvRow = (GridDataItem)ucQuick.Parent.Parent;

        if (gvRow.FindControl("ddlRemarksAdd") != null)
        {
            RadComboBox ddlRemarks = (RadComboBox)gvRow.FindControl("ddlRemarksAdd");
            if (General.GetNullableInteger(ucQuick.SelectedQuick).HasValue && (General.GetNullableString(ucQuick.SelectedText.ToUpper()) == "REPLACEMENT"))
            {
                ddlRemarks.CssClass = "input_mandatory";
            }
            else
            {
                ddlRemarks.CssClass = "input";
            }
        }//? "ddlRemarksAdd" : "ddlRemarksEdit") as DropDownList;
        if (gvRow.FindControl("ddlRemarksEdit") != null)
        {
            RadComboBox ddlRemarks = (RadComboBox)gvRow.FindControl("ddlRemarksEdit");
            if (General.GetNullableInteger(ucQuick.SelectedQuick).HasValue && (General.GetNullableString(ucQuick.SelectedText.ToUpper()) == "REPLACEMENT"))
            {
                ddlRemarks.CssClass = "input_mandatory";
            }
            else
            {
                ddlRemarks.CssClass = "input";
            }
        }


    }
    protected void ucReasonAdd_Changed(object sender, EventArgs e)
    {
        UserControlQuick ucQuick = (UserControlQuick)sender;
        GridFooterItem gvRow = (GridFooterItem)ucQuick.Parent.Parent;
        RadComboBox ddlRemarks = gvRow.FindControl("ddlRemarksAdd") as RadComboBox;

        if (General.GetNullableInteger(ucQuick.SelectedQuick).HasValue && (General.GetNullableString(ucQuick.SelectedText.ToUpper()) == "REPLACEMENT"))
        {
            ddlRemarks.CssClass = "input_mandatory";
        }
        else
        {
            ddlRemarks.CssClass = "input";
        }
    }
    protected void RemarksEdit_Changed(object sender, EventArgs e)
    {
        RadComboBox ddlRemarks = (RadComboBox)sender;
        GridDataItem gvRow = (GridDataItem)ddlRemarks.Parent.Parent;
        RadLabel lblSealUsageIdEdit = gvRow.FindControl("lblSealUsageIdEdit") as RadLabel;
    }
    protected void RemarksAdd_Changed(object sender, EventArgs e)
    {
        RadComboBox ddlRemarks = (RadComboBox)sender;
        //GridViewRow gvRow = (GridViewRow)ddlRemarks.Parent.Parent;
    }

    protected void gvSealUsage_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealUsage.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealUsage_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {


        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton imgRem = (LinkButton)e.Item.FindControl("cmdViewChangeRemarks");
        if (imgRem != null) imgRem.Visible = SessionUtil.CanAccess(this.ViewState, imgRem.CommandName);


        if (e.Item is GridDataItem)
        {

            LinkButton btnShowSealNumberEdit = (LinkButton)e.Item.FindControl("btnShowSealNumberEdit");
            if (btnShowSealNumberEdit != null)
            {
                string status = "";
                status = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");
                btnShowSealNumberEdit.Attributes.Add("onclick", "return showPickList('spnPickListSealNumberedit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSealNumber.aspx?iframignore=true&status=" + status + "', true); ");
            }
            RadTextBox txtSealNumberEdit = (RadTextBox)e.Item.FindControl("txtSealNumberEdit");
            if (txtSealNumberEdit != null)
                txtSealNumberEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDSEALNO").ToString();
            RadTextBox txtSealidEdit = (RadTextBox)e.Item.FindControl("txtSealidEdit");
            if (txtSealidEdit != null)
                txtSealidEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDSEALID").ToString();

            LinkButton btnSealAffixedbyEdit = (LinkButton)e.Item.FindControl("btnSealAffixedbyEdit");
            if (btnSealAffixedbyEdit != null)
            {
                btnSealAffixedbyEdit.Attributes.Add("onclick", "return showPickList('spnSealAffixedByEdit', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
            }
            RadTextBox txtSealAffixedbyNameEdit = (RadTextBox)e.Item.FindControl("txtSealAffixedbyNameEdit");
            if (txtSealAffixedbyNameEdit != null) txtSealAffixedbyNameEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDPERSONAFFIXINGSEAL").ToString();
            RadTextBox txtSealAffixedbyRankEdit = (RadTextBox)e.Item.FindControl("txtSealAffixedbyRankEdit");
            if (txtSealAffixedbyRankEdit != null) txtSealAffixedbyRankEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDRANKNAME").ToString();
            RadTextBox txtSealAffixedbyIdEdit = (RadTextBox)e.Item.FindControl("txtSealAffixedbyIdEdit");
            if (txtSealAffixedbyIdEdit != null) txtSealAffixedbyIdEdit.Text = DataBinder.Eval(e.Item.DataItem, "FLDEMPLOYEEID").ToString();

            UserControlQuick ucReasonEdit = (UserControlQuick)e.Item.FindControl("ucReasonEdit");
            RadComboBox ddlRemarks = (RadComboBox)e.Item.FindControl("ddlRemarksEdit");
            if (ddlRemarks != null)
            {
                ddlRemarks.DataSource = PhoenixInspectionSealUsage.ListSealUsageRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null);
                ddlRemarks.DataTextField = "FLDUSAGEREMARKSNAME";
                ddlRemarks.DataValueField = "FLDSEALUSAGEREMARKSID";
                ddlRemarks.DataBind();

            }
            if (ucReasonEdit != null && ddlRemarks != null)
            {
                ucReasonEdit.bind();
                ucReasonEdit.SelectedQuick = DataBinder.Eval(e.Item.DataItem, "FLDREASON").ToString();

                ddlRemarks.SelectedValue = DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString();
                //if (ucReasonEdit.SelectedText.ToUpper() == "REPLACEMENT")
                //{
                //    ddlRemarks.CssClass = "input_mandatory";
                //}
                //else
                //{
                //    ddlRemarks.CssClass = "input";
                //}
                if (DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "4" || DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "5" || DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "6")
                {
                    NameValueCollection nvc = new NameValueCollection();
                    if (Filter.CurrentSealUsageRemarks == null)
                    {
                        nvc.Add("sealusageid", DataBinder.Eval(e.Item.DataItem, "FLDSEALUSAGEID").ToString());
                        nvc.Add("ucMultiInspection", DataBinder.Eval(e.Item.DataItem, "FLDUSAGEINSPECTION").ToString());
                        nvc.Add("ucMultiIncident", DataBinder.Eval(e.Item.DataItem, "FLDUSAGEINCIDENT").ToString());
                        nvc.Add("txtRemarks", DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKS").ToString());
                        Filter.CurrentSealUsageRemarks = nvc;
                    }
                }
            }
            LinkButton history = (LinkButton)e.Item.FindControl("cmdHistory");
            RadLabel lblSealUsageId = (RadLabel)e.Item.FindControl("lblSealUsageId");
            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
            if (history != null && lblSealUsageId != null)
                history.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageHistory.aspx?SEALUSAGEID=" + lblSealUsageId.Text + "',null)");
            RadLabel lblSealUsageIdEdit = (RadLabel)e.Item.FindControl("lblSealUsageIdEdit");

            if (lblRemarks != null)
            {
                if (General.GetNullableString(lblRemarks.Text) != null)
                {
                    UserControlToolTip ucToolTipRemarks = (UserControlToolTip)e.Item.FindControl("ucToolTipUsageRemarks");
                    lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipRemarks.ToolTip + "', 'visible');");
                    lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipRemarks.ToolTip + "', 'hidden');");
                }
            }
            if (imgRem != null)
            {
                if (DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "4" || DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "5" || DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() == "6")
                {
                    imgRem.Visible = true;
                    imgRem.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageRemarks.aspx?type=2&sealusageid=" + DataBinder.Eval(e.Item.DataItem, "FLDSEALUSAGEID").ToString() + "&remarksid=" + DataBinder.Eval(e.Item.DataItem, "FLDUSAGEREMARKSID").ToString() + "','xlarge')");
                }
                else
                {
                    imgRem.Visible = false;
                }
            }


            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
            if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("3"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                imgFlag.ToolTip = "Replacement Overdue";
            }
            else if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("2"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                imgFlag.ToolTip = "Replacement Due within 30 days";
            }
            else if (imgFlag != null && DataBinder.Eval(e.Item.DataItem, "FLDREPLACEMENTDUE").ToString().Equals("1"))
            {
                imgFlag.Visible = true;
                imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
                imgFlag.ToolTip = "Replacement Due within 60 days";
            }
            else
            {
                if (imgFlag != null) imgFlag.Visible = false;
            }

        }
        if (e.Item is GridFooterItem)
        {
            RadComboBox ddlLocationAdd = (RadComboBox)e.Item.FindControl("ddlLocationAdd");

            if (ddlLocationAdd != null)
            {
                ddlLocationAdd.DataSource = PhoenixInspectionSealLocation.SealLocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ddlLocationAdd.DataTextField = "FLDLOCATIONNAME";
                ddlLocationAdd.DataValueField = "FLDLOCATIONID";
                ddlLocationAdd.DataBind();
                ddlLocationAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }
            LinkButton btnShowSealNumberAdd = (LinkButton)e.Item.FindControl("btnShowSealNumberAdd");
            if (btnShowSealNumberAdd != null)
            {
                string status = "";
                status = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");
                btnShowSealNumberAdd.Attributes.Add("onclick", "return showPickList('spnPickListSealNumberAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSealNumber.aspx?iframignore=true&status=" + status + "', true); ");
            }

            LinkButton btnSealAffixedbyAdd = (LinkButton)e.Item.FindControl("btnSealAffixedbyAdd");
            if (btnSealAffixedbyAdd != null)
            {
                btnSealAffixedbyAdd.Attributes.Add("onclick", "return showPickList('spnSealAffixedByAdd', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                    + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "', true); ");
            }

            UserControlQuick ucReasonAdd = (UserControlQuick)e.Item.FindControl("ucReasonAdd");
            if (ucReasonAdd != null)
                ucReasonAdd.bind();

            UserControlDate txtAffixedDateAdd = (UserControlDate)e.Item.FindControl("txtAffixedDateAdd");
            if (txtAffixedDateAdd != null)
            {
                txtAffixedDateAdd.Text = DateTime.Today.ToString();
            }

            RadComboBox ddlRemarks = (RadComboBox)e.Item.FindControl("ddlRemarksAdd");
            if (ddlRemarks != null)
            {
                ddlRemarks.DataSource = PhoenixInspectionSealUsage.ListSealUsageRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , null
                    , null);
                ddlRemarks.DataTextField = "FLDUSAGEREMARKSNAME";
                ddlRemarks.DataValueField = "FLDSEALUSAGEREMARKSID";
                ddlRemarks.DataBind();
                ddlRemarks.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            }

            //if (ucReasonAdd != null && ddlRemarks != null)
            //{

            //    if (ucReasonAdd.SelectedText != null && ucReasonAdd.SelectedText.ToUpper() == "REPLACEMENT")
            //    {
            //        ddlRemarks.CssClass = "input_mandatory";
            //    }
            //    else
            //    {
            //        ddlRemarks.CssClass = "input";
            //    }
            //}
        }

    }

    protected void gvSealUsage_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 || General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) == null)
                {
                    ucError.ErrorMessage = "Please switch to vessel.";
                    ucError.Visible = true;
                    return;
                }
                string loc = ((RadComboBox)e.Item.FindControl("ddlLocationAdd")).SelectedValue != null ? ((RadComboBox)e.Item.FindControl("ddlLocationAdd")).SelectedValue : "";
                string remark = ((RadComboBox)e.Item.FindControl("ddlRemarksAdd")).SelectedValue != null ? ((RadComboBox)e.Item.FindControl("ddlRemarksAdd")).SelectedValue : "";
                string reason = ((UserControlQuick)e.Item.FindControl("ucReasonAdd")).SelectedQuick != null ? ((UserControlQuick)e.Item.FindControl("ucReasonAdd")).SelectedQuick : "";
                string reasontext = reason != "" ? ((UserControlQuick)e.Item.FindControl("ucReasonAdd")).SelectedText : "";
                if (!IsValidSealUsage(loc
                    , ((RadTextBox)e.Item.FindControl("txtSealidAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSealAffixedbyIdAdd")).Text
                    , ((UserControlDate)e.Item.FindControl("txtAffixedDateAdd")).Text
                    , reason
                    , reasontext
                    , remark))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection nvcRem = new NameValueCollection();
                nvcRem = null;
                if (Filter.CurrentSealUsageRemarks != null)
                {
                    nvcRem = Filter.CurrentSealUsageRemarks;
                }

                Guid? newinsertedid = null;
                PhoenixInspectionSealUsage.InsertSealUsage(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , int.Parse(((RadComboBox)e.Item.FindControl("ddlLocationAdd")).SelectedValue.ToString())
                    , new Guid(((RadTextBox)e.Item.FindControl("txtSealidAdd")).Text)
                    , int.Parse(((RadTextBox)e.Item.FindControl("txtSealAffixedbyIdAdd")).Text)
                    , DateTime.Parse(((UserControlDate)e.Item.FindControl("txtAffixedDateAdd")).Text)
                    , int.Parse(((UserControlQuick)e.Item.FindControl("ucReasonAdd")).SelectedQuick)
                    , ref newinsertedid
                    , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlRemarksAdd")).SelectedValue)
                    , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiInspection"].ToString()) : null
                    , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiIncident"].ToString()) : null
                    , nvcRem != null ? General.GetNullableString(nvcRem["txtRemarks"].ToString()) : null);

                PhoenixInspectionSealUsage.InsertSealUsageHistory(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , int.Parse(((RadComboBox)e.Item.FindControl("ddlLocationAdd")).SelectedValue.ToString())
                    , new Guid(((RadTextBox)e.Item.FindControl("txtSealidAdd")).Text)
                    , int.Parse(((RadTextBox)e.Item.FindControl("txtSealAffixedbyIdAdd")).Text)
                    , DateTime.Parse(((UserControlDate)e.Item.FindControl("txtAffixedDateAdd")).Text)
                    , int.Parse(((UserControlQuick)e.Item.FindControl("ucReasonAdd")).SelectedQuick)
                    , newinsertedid);

                //BindData();
                gvSealUsage.Rebind();
                //SetPageNavigator();
            }
            if(e.CommandName.ToUpper()=="UPDATE")
            {
                try
                {

                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 || General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) == null)
                    {
                        ucError.ErrorMessage = "Please switch to vessel.";
                        ucError.Visible = true;
                        return;
                    }

                    if (!IsValidSealUsage(((RadLabel)e.Item.FindControl("lblLocationidEdit")).Text
                            , ((RadTextBox)e.Item.FindControl("txtSealidEdit")).Text
                            , ((RadTextBox)e.Item.FindControl("txtSealAffixedbyIdEdit")).Text
                            , ((UserControlDate)e.Item.FindControl("txtAffixedDateEdit")).Text
                            , ((UserControlQuick)e.Item.FindControl("ucReasonEdit")).SelectedQuick
                            , ((UserControlQuick)e.Item.FindControl("ucReasonEdit")).SelectedText
                            , ((RadComboBox)e.Item.FindControl("ddlRemarksEdit")).SelectedValue
                            ))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    NameValueCollection nvcRem = new NameValueCollection();
                    nvcRem = null;
                    if (Filter.CurrentSealUsageRemarks != null)
                    {
                        nvcRem = Filter.CurrentSealUsageRemarks;
                    }
                    PhoenixInspectionSealUsage.UpdateSealUsage(new Guid(((RadLabel)e.Item.FindControl("lblSealUsageIdEdit")).Text)
                            , new Guid(((RadTextBox)e.Item.FindControl("txtSealidEdit")).Text)
                            , int.Parse(((RadTextBox)e.Item.FindControl("txtSealAffixedbyIdEdit")).Text)
                            , DateTime.Parse(((UserControlDate)e.Item.FindControl("txtAffixedDateEdit")).Text)
                            , int.Parse(((UserControlQuick)e.Item.FindControl("ucReasonEdit")).SelectedQuick)
                            , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlRemarksEdit")).SelectedValue)
                            , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiInspection"].ToString()) : null
                            , nvcRem != null ? General.GetNullableGuid(nvcRem["ucMultiIncident"].ToString()) : null
                            , nvcRem != null ? General.GetNullableString(nvcRem["txtRemarks"].ToString()) : null);
                    Filter.CurrentSealUsageRemarks = null;


                    //BindData();
                    gvSealUsage.Rebind();

                    // SetPageNavigator();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvSealUsage_SortCommand(object sender, GridSortCommandEventArgs e)
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


    protected void ddlRemarksAdd_TextChanged(object sender, EventArgs e)
    {
        RadComboBox dc = (RadComboBox)sender;
        if (dc.ID == "ddlRemarksEdit")
        {
            GridDataItem dataItem = (GridDataItem)dc.NamingContainer;
            string remarksid = dc.SelectedValue;
            RadLabel lblIdEdit = (RadLabel)dataItem.FindControl("lblIdEdit");
            string sealusageid = lblIdEdit.Text;
            if (remarksid == "4" || remarksid == "5" || remarksid == "6")
            {
                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=" + sealusageid + "&remarksid=" + remarksid + "');");
                // String.Format("'codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=" + sealusageid + "&remarksid=" + remarksid +"', 'xlarge'); return false;");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                //javascript: parent.Openpopup('codehelp1', '', '../Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=' + sealusageid + '&remarksid=' + remarksid, 'xlarge'); return false;
            }
        }
        if(dc.ID== "ddlRemarksAdd")
        {
            GridFooterItem dataItem = (GridFooterItem)dc.NamingContainer;
            string remarksid = dc.SelectedValue;
           
          
            if (remarksid == "4" || remarksid == "5" || remarksid == "6")
            {
                String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageRemarks.aspx?type=1&remarksid=" + remarksid + "');");
                // String.Format("'codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=" + sealusageid + "&remarksid=" + remarksid +"', 'xlarge'); return false;");

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                //javascript: parent.Openpopup('codehelp1', '', '../Inspection/InspectionSealUsageRemarks.aspx?type=1&sealusageid=' + sealusageid + '&remarksid=' + remarksid, 'xlarge'); return false;
            }
            
        }

    }
}
