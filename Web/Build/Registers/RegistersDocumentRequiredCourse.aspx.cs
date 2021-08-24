using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDocumentRequiredCourse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden"); <i class="fas fa-envelope-open-text"></i>
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvDocumentsRequired')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar1.AddFontAwesomeButton("javascript:openNewWindow('expired','','" + Session["sitepath"] + "/Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString(), "Copy", "<i class=\"fas fa-copy\"></i>", "COPY");
        toolbar1.AddFontAwesomeButton("javascript:openNewWindow('expired','','" + Session["sitepath"] + "/Registers/RegistersDocumentRequiredCourseCopy.aspx?flag="
                                + ucFlag.SelectedFlag + "&vesseltype=" + ucVesselType.SelectedVesseltype + "&rank=" + ucRank.SelectedRank + "')", "Copy", "<i class=\"fas fa-copy\"></i>", "COPY");
        toolbar1.AddFontAwesomeButton("javascript:openNewWindow('expired','','" + Session["sitepath"] + "/Registers/RegistersDocumentRequiredCourseArchived.aspx?vesselid=" + Filter.CurrentVesselMasterFilter + "'); return false;", "Show Archived", "<i class=\"fas fa-envelope-open-text\"></i>", "ARCHIVED");
        MenuRegistersDocumentsRequired.AccessRights = this.ViewState;
        MenuRegistersDocumentsRequired.MenuList = toolbar1.Show();
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);
        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);

        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();
       
        MenuVesselList.SelectedMenuIndex = 1;

        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        ViewState["CURRENTINDEX"] = 1;
        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2 = new PhoenixToolbar();
        toolbar2.AddButton("Licence", "LICENCE", ToolBarDirection.Right);
        toolbar2.AddButton("Course", "COURSE", ToolBarDirection.Right);

        if (!IsPostBack)
        {

            if (Request.QueryString["launchedfrom"].ToString().ToUpper().Equals("VESSEL"))
            {
                DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
                DataRow drVessel = dsVessel.Tables[0].Rows[0];
                txtVesselName.Text = drVessel["FLDVESSELNAME"].ToString();
                ucFlag.SelectedFlag = drVessel["FLDFLAG"].ToString();
                ucVesselType.SelectedVesseltype = drVessel["FLDTYPE"].ToString();
                ucFlag.Enabled = false;
                ucVesselType.Enabled = false;
                PhoenixRegistersVessel.TqToVesselCourseCopy(General.GetNullableInteger(drVessel["FLDVESSELID"].ToString()));
            }
            else
            {
                Filter.CurrentVesselMasterFilter = "";
                DataSet ds = PhoenixRegistersFlag.EditFlag(int.Parse(Session["FLAGID"].ToString()));
                ucFlag.SelectedFlag = ds.Tables[0].Rows[0]["FLDCOUNTRYCODE"].ToString();
                ucFlag.Enabled = false;
            }
            MenuFlag.AccessRights = this.ViewState;
            MenuFlag.MenuList = toolbar2.Show();
            MenuFlag.SelectedMenuIndex = 1;
            gvDocumentsRequired.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
           

        }
    }
    protected void Rebind()
    {
        gvDocumentsRequired.SelectedIndexes.Clear();
        gvDocumentsRequired.EditIndexes.Clear();
        gvDocumentsRequired.DataSource = null;
        gvDocumentsRequired.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDDOCUMENTNAME", "FLDSET", "FLDEFFECTIVEDATE" };
        string[] alCaptions = { "Document Type", "Course", "Set", "Effective From" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselDocumentCourse.DocumentsRequiredSearch(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                 General.GetNullableInteger(ucRank.SelectedRank),
                 General.GetNullableInteger(ucFlag.SelectedFlag),
                 General.GetNullableInteger(ucVesselType.SelectedVesseltype), General.GetNullableInteger(ucDocumentType.SelectedHard), 1,
                 sortexpression, sortdirection,
                 Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                 General.ShowRecords(null),
                 ref iRowCount,
                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentsRequiredCourse.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Documents Required (Course)</h3></td>");
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

    protected void RegistersDocumentsRequired_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper() == "COPY")
        {
            String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1','','../Registers/RegistersDocumentRequiredCourseCopy.aspx?flag="
                                   + ucFlag.SelectedFlag + "&vesseltype=" + ucVesselType.SelectedVesseltype + "&rank=" + ucRank.SelectedRank + " ');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

        }

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDOCUMENTTYPENAME", "FLDDOCUMENTNAME" };
        string[] alCaptions = { "Document Type", "Course" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselDocumentCourse.DocumentsRequiredSearch(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                General.GetNullableInteger(ucRank.SelectedRank), General.GetNullableInteger(ucFlag.SelectedFlag),
                General.GetNullableInteger(ucVesselType.SelectedVesseltype), General.GetNullableInteger(ucDocumentType.SelectedHard), 1,
                sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvDocumentsRequired.PageSize,
                ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentsRequired", "Document Course", alCaptions, alColumns, ds);

        gvDocumentsRequired.DataSource = ds;
        gvDocumentsRequired.VirtualItemCount = iRowCount;

        //if (gvDocumentsRequired.EditIndex == -1)
        //{
        //    UserControlCourse ddlCourseAdd = (UserControlCourse)gvDocumentsRequired.FooterRow.FindControl("ucCourseAdd");
        //    ddlCourseAdd.CourseList = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(ucDocumentType.SelectedHard));
        //}
        //else
        //{
        //    UserControlCourse ddlCourseEdit = (UserControlCourse)gvDocumentsRequired.Rows[gvDocumentsRequired.EditIndex].FindControl("ucCourseEdit");
        //    ddlCourseEdit.CourseList = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(ucDocumentType.SelectedHard));
        //    ddlCourseEdit.SelectedCourse = ((Label)gvDocumentsRequired.Rows[gvDocumentsRequired.EditIndex].FindControl("lblDocumentEditId")).Text;
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void Flag_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LICENCE"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredLicence.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("../Registers/RegistersVesselMedicalTestMap.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
        }
        else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            Response.Redirect("../Registers/RegistersVesselCorrespondence.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CHATBOX"))
        {
            Response.Redirect("../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter, false);
        }
    }
    protected void gvDocumentsRequired_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton alternaterank = (LinkButton)e.Item.FindControl("cmdAltRnk");
            if (alternaterank != null)
            {
                alternaterank.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersRequiredCourseAlternateRank.aspx?vslDocId=" + drv["FLDVESSELDOCUMENTID"].ToString() + "&rankId=" + ucRank.SelectedValue + "');");
            }

            UserControlCourse ucCourse = (UserControlCourse)e.Item.FindControl("ucCourseEdit");

            if (ucCourse != null) ucCourse.SelectedCourse = drv["FLDDOCUMENTID"].ToString();

            UserControlDate ucEffectiveDate = (UserControlDate)e.Item.FindControl("txtEffectiveDateEdit");
            if (ucEffectiveDate != null) ucEffectiveDate.Text = drv["FLDEFFECTIVEDATE"].ToString();

            UserControlDate ucValidTillDate = (UserControlDate)e.Item.FindControl("txtValidTillDateEdit");
            if (ucValidTillDate != null) ucValidTillDate.Text = drv["FLDVALIDTILLDATE"].ToString();
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

        }
    }
    protected void gvDocumentsRequired_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertDocumentsRequired(Filter.CurrentVesselMasterFilter
                     , ucRank.SelectedRank
                     , ((UserControlCourse)e.Item.FindControl("ucCourseAdd")).SelectedCourse
                     , ((RadTextBox)e.Item.FindControl("txtSetAdd")).Text
                     , ucFlag.SelectedFlag
                     , ucVesselType.SelectedVesseltype
                     , (((RadCheckBox)e.Item.FindControl("chkFlagEndYNAdd")).Checked) == true ? 1 : 0
                     , (((RadCheckBox)e.Item.FindControl("chkMandatoryYNAdd")).Checked == true ? 1 : 0)
                     , ((UserControlDate)e.Item.FindControl("txtEffectiveDateAdd")).Text
                     , ((UserControlDate)e.Item.FindControl("txtValidTillDateAdd")).Text
                     );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateDocumentsRequired(
                    ((RadLabel)e.Item.FindControl("lblVesselDocumentIdEdit")).Text
                    , Filter.CurrentVesselMasterFilter
                    , ucRank.SelectedRank
                    , ((UserControlCourse)e.Item.FindControl("ucCourseEdit")).SelectedCourse
                    , ((RadTextBox)e.Item.FindControl("txtSetEdit")).Text
                    , ucFlag.SelectedFlag
                    , ucVesselType.SelectedVesseltype
                    , (((RadCheckBox)e.Item.FindControl("chkFlagEndYNEdit")).Checked) == true ? 1 : 0
                    , (((RadCheckBox)e.Item.FindControl("chkMandatoryYNEdit")).Checked) == true ? 1 : 0
                    , ((UserControlDate)e.Item.FindControl("txtEffectiveDateEdit")).Text
                    , ((UserControlDate)e.Item.FindControl("txtValidTillDateEdit")).Text
                    );


                ucStatus.Text = "Document information updated";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                DeleteDocumentsRequired(Int32.Parse(((RadLabel)e.Item.FindControl("lblVesselDocumentId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "ARCHIVE")
            {
                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text);
                string effectivedate = ((RadLabel)e.Item.FindControl("lblEffectiveDate")).Text;
                PhoenixRegistersVesselDocumentCourse.ArchiveDocumentsRequired(Int16.Parse(Filter.CurrentVesselMasterFilter), dtkey, 0, General.GetNullableDateTime(effectivedate), null);
            }
            else if (e.CommandName == "Page")
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
    protected void gvDocumentsRequired_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDocumentsRequired.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertDocumentsRequired(string vesselid, string rank, string documentid, string set
                                            , string flagid, string vesseltypeid, int flagendyn, int optionalyn, string effectivedate, string validtilldate)
    {
        if (!IsValidDocumentsRequired(rank, documentid, set))
        {
            ucError.Visible = true;
            return;
        }
        else
        {
            PhoenixRegistersVesselDocumentCourse.InsertDocumentsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(Filter.CurrentVesselMasterFilter), Convert.ToInt32(rank), Convert.ToInt32(documentid), set
                , Convert.ToInt32(flagid), Convert.ToInt32(vesseltypeid), flagendyn, optionalyn, General.GetNullableDateTime(effectivedate), General.GetNullableDateTime(validtilldate));
        }
    }
    private void UpdateDocumentsRequired(string vesseldocumentd, string vesselid, string rank, string documentid, string set
       , string flagid, string vesseltypeid, int flagendyn, int optionalyn, string effectivedate, string validtilldate)
    {
        if (!IsValidDocumentsRequired(rank, documentid, set))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersVesselDocumentCourse.UpdateDocumentsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Convert.ToInt32(vesseldocumentd)
                , General.GetNullableInteger(Filter.CurrentVesselMasterFilter), Convert.ToInt32(rank), Convert.ToInt32(documentid), set, Convert.ToInt32(flagid)
                , Convert.ToInt32(vesseltypeid), flagendyn, optionalyn, General.GetNullableDateTime(effectivedate), General.GetNullableDateTime(validtilldate));
    }
    private bool IsValidDocumentsRequired(string rank, string documentid, string set)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (rank.Trim().Equals("0") || !Int16.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableInteger(documentid) == null)
            ucError.ErrorMessage = "Document Name is required.";

        if (General.GetNullableInteger(ucFlag.SelectedFlag) == null)
            ucError.ErrorMessage = "Flag is required";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        if (General.GetNullableString(set) == null)
            ucError.ErrorMessage = "Set is required.";

        return (!ucError.IsError);
    }
    private void DeleteDocumentsRequired(int vesseldocumentd)
    {
        PhoenixRegistersVesselDocumentCourse.DeleteDocumentsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesseldocumentd);
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }
}
