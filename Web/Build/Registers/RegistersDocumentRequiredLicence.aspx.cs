using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersDocumentRequiredLicence : PhoenixBasePage
{
    private static int documenttypeid = (int)PhoenixDocumentType.LICENCE;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Licence", "LICENCE", ToolBarDirection.Right);
        toolbar1.AddButton("Course", "COURSE", ToolBarDirection.Right);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        // toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);


        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            MenuVesselList.SelectedMenuIndex = 1;
        else
            MenuVesselList.SelectedMenuIndex = 1;

        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddFontAwesomeButton("../Registers/RegistersDocumentRequiredLicence.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvDocumentsRequired')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar2.AddFontAwesomeButton("javascript:openNewWindow('expired','','" + Session["sitepath"] + "/Registers/RegistersDocumentRequiredLicenceCopyToFilter.aspx?flag="
            + ucFlag.SelectedFlag + "&vesseltype=" + ucVesselType.SelectedVesseltype + "')", "Copy", "<i class=\"fas fa-copy\"></i>", "COPY");

        MenuRegistersDocumentsRequired.AccessRights = this.ViewState;
        MenuRegistersDocumentsRequired.MenuList = toolbar2.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["launchedfrom"].ToString().ToUpper().Equals("VESSEL"))
            {
                DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
                DataRow drVessel = dsVessel.Tables[0].Rows[0];
                txtVesselName.Text = drVessel["FLDVESSELNAME"].ToString();
                ucFlag.SelectedFlag = drVessel["FLDFLAG"].ToString();
                ucVesselType.SelectedVesseltype = drVessel["FLDTYPE"].ToString();
                ucFlag.Enabled = false;
                ucVesselType.Enabled = false;
            }
            else
            {
                Filter.CurrentVesselMasterFilter = "";
                DataSet ds = PhoenixRegistersFlag.EditFlag(int.Parse(Session["FLAGID"].ToString()));
                ucFlag.SelectedFlag = ds.Tables[0].Rows[0]["FLDCOUNTRYCODE"].ToString();
                ucFlag.Enabled = false;
            }

            MenuFlag.AccessRights = this.ViewState;
            MenuFlag.MenuList = toolbar1.Show();
            MenuFlag.SelectedMenuIndex = 0;
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

        string[] alColumns = { "FLDTYPENAME", "DOCUMENTNAME", "FLDSET" };
        string[] alCaptions = { "Document Type", "Document Name", "Set" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselDocumentsRequired.DocumentsRequiredSearch(
            null
            , (General.GetNullableInteger(ucRank.SelectedRank) == null ? 0 : General.GetNullableInteger(ucRank.SelectedRank))
            , (General.GetNullableInteger(ucFlag.SelectedFlag) == null ? 0 : General.GetNullableInteger(ucFlag.SelectedFlag))
            , (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null ? 0 : General.GetNullableInteger(ucVesselType.SelectedVesseltype))
            , documenttypeid
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentsRequiredLicence.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Documents Required (Licence)</h3></td>");
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
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTYPENAME", "DOCUMENTNAME", "FLDSET" };
        string[] alCaptions = { "Document Type", "Document Name", "Set" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVesselDocumentsRequired.DocumentsRequiredSearch(
            null
            , (General.GetNullableInteger(ucRank.SelectedRank) == null ? 0 : General.GetNullableInteger(ucRank.SelectedRank))
            , (General.GetNullableInteger(ucFlag.SelectedFlag) == null ? 0 : General.GetNullableInteger(ucFlag.SelectedFlag))
            , (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null ? 0 : General.GetNullableInteger(ucVesselType.SelectedVesseltype))
            , documenttypeid, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvDocumentsRequired.PageSize,
            ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDocumentsRequired", "Documents Required (Licence)", alCaptions, alColumns, ds);
        gvDocumentsRequired.DataSource = ds;
        gvDocumentsRequired.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Flag_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COURSE"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("../Registers/RegistersDocumentRequiredMedical.aspx?launchedfrom=" + Request.QueryString["launchedfrom"].ToString());
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
    protected void gvDocumentsRequired_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                InsertDocumentsRequired(Filter.CurrentVesselMasterFilter
                      , ucRank.SelectedRank
                      , ((UserControlHard)e.Item.FindControl("ucDocumentTypeAdd")).SelectedHard
                      , ((UserControlDocuments)e.Item.FindControl("ucDocumentsAdd")).SelectedDocument
                      , ((RadTextBox)e.Item.FindControl("txtSetAdd")).Text
                      , ucFlag.SelectedFlag
                      , ucVesselType.SelectedVesseltype
                      , (((RadCheckBox)e.Item.FindControl("chkFlagEndYNAdd")).Checked) == true ? 1 : 0
                      , (((RadCheckBox)e.Item.FindControl("chkPaidByOwnerAdd")).Checked == true ? 1 : 0));
                Rebind();
                ucStatus.Text = "Document information Added";
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateDocumentsRequired(
                    ((RadLabel)e.Item.FindControl("lblVesselDocumentIdEdit")).Text
                    , Filter.CurrentVesselMasterFilter
                    , ucRank.SelectedRank
                    , ((UserControlHard)e.Item.FindControl("ucDocumentTypeEdit")).SelectedHard
                    , ((UserControlDocuments)e.Item.FindControl("ucDocumentsEdit")).SelectedDocument
                    , ((RadTextBox)e.Item.FindControl("txtSetEdit")).Text
                    , ucFlag.SelectedFlag
                    , ucVesselType.SelectedVesseltype
                    , (((RadCheckBox)e.Item.FindControl("chkFlagEndYNEdit")).Checked) == true ? 1 : 0
                    , (((RadCheckBox)e.Item.FindControl("chkPaidByOwnerEdit")).Checked) == true ? 1 : 0);
                Rebind();
                ucStatus.Text = "Document information updated";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocumentsRequired(Int32.Parse(((RadLabel)e.Item.FindControl("lblVesselDocumentId")).Text));
                Rebind();
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

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkType");
            if (lb != null)
            {
                lb.Enabled = SessionUtil.CanAccess(this.ViewState, lb.CommandName);
            }

            UserControlDocuments ucDocuments = (UserControlDocuments)e.Item.FindControl("ucDocumentsEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDDOCUMENTID"].ToString();

            UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucDocumentTypeEdit");
            DataRowView drvHard = (DataRowView)e.Item.DataItem;
            if (ucHard != null) ucHard.SelectedHard = drvHard["FLDTYPE"].ToString();

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
    private void InsertDocumentsRequired(string vesselid, string rank, string type, string documentid
        , string set, string flagid, string vesseltypeid, int flagendyn, int paidbywoner)
    {
        if (!IsValidDocumentsRequired(rank, type, documentid))
        {
            ucError.Visible = true;
            return;
        }
        else
        {
            DataSet ds = PhoenixRegistersVesselDocumentsRequired.ListDocumentsRequired();

            PhoenixRegistersVesselDocumentsRequired.InsertDocumentsRequired(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , null
                , Convert.ToInt16(rank)
                , Convert.ToInt16(type)
                , Convert.ToInt16(documentid)
                , Convert.ToInt16(flagid)
                , Convert.ToInt16(vesseltypeid)
                , flagendyn
                , paidbywoner);
        }
    }
    private void UpdateDocumentsRequired(string vesseldocumentd, string vesselid, string rank, string type, string documentid
        , string set, string flagid, string vesseltypeid, int flagendyn, int paidbyowner)
    {
        if (!IsValidDocumentsRequired(rank, type, documentid))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersVesselDocumentsRequired.UpdateDocumentsRequired(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Convert.ToInt16(vesseldocumentd)
            , null
            , Convert.ToInt16(rank)
            , Convert.ToInt16(type)
            , Convert.ToInt16(documentid)
            , Convert.ToInt16(flagid)
            , Convert.ToInt16(vesseltypeid)
            , flagendyn
            , paidbyowner);
    }
    private bool IsValidDocumentsRequired(string rank, string type, string documentid)
    {
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (rank.Trim().Equals("0") || !Int16.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";

        if (documentid.Trim().Equals("") || !Int16.TryParse(documentid, out resultInt))
            ucError.ErrorMessage = "Document Name is required.";

        if (type.Trim().Equals("") || !Int16.TryParse(type, out resultInt))
            ucError.ErrorMessage = "Document Type is required.";

        if (General.GetNullableInteger(ucFlag.SelectedFlag) == null)
            ucError.ErrorMessage = "Flag is required";

        if (General.GetNullableInteger(ucVesselType.SelectedVesseltype) == null)
            ucError.ErrorMessage = "Vessel Type is required.";

        return (!ucError.IsError);
    }
    private void DeleteDocumentsRequired(int vesseldocumentd)
    {
        PhoenixRegistersVesselDocumentsRequired.DeleteDocumentsRequired(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , vesseldocumentd);
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

    protected void ucRank_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
