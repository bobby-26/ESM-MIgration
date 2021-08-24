using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeLicence : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            MenuActivityFilterMain.AccessRights = this.ViewState;
            MenuActivityFilterMain.MenuList = toolbarmain.Show();
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentVesselCrewSelection = Request.QueryString["empid"];
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeLicence.aspx?e=1", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewLicence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewLicence.AccessRights = this.ViewState;
            MenuCrewLicence.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeLicence.aspx?e=2", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFlagEndorsement')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewFE.AccessRights = this.ViewState;
            CrewFE.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsEmployeeLicence.aspx?e=3", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDCE')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewDCE.AccessRights = this.ViewState;
            CrewDCE.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersThirdPartyLinks.EditThirdPartyLinks(2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cocChecker.HRef = ds.Tables[0].Rows[0]["FLDLINKNAME"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["FEPAGENUMBER"] = 1;
                ViewState["FESORTEXPRESSION"] = null;
                ViewState["FESORTDIRECTION"] = null;
                ViewState["FECURRENTINDEX"] = 1;
                ViewState["DCEPAGENUMBER"] = 1;
                ViewState["DCESORTEXPRESSION"] = null;
                ViewState["DCESORTDIRECTION"] = null;
                ViewState["DCECURRENTINDEX"] = 1;
                ViewState["IEMPLICENCEID"] = null;
                SetEmployeePrimaryDetails();
                gvCrewLicence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
            BindFEData();
            BindDCEData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewLicence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvCrewLicence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToString().ToUpper() == "VIEW" || e.CommandName.ToString().ToUpper() == "SELECT")
            {
                RadLabel LidEdit = ((RadLabel)eeditedItem.FindControl("lblLicenceIdEdit"));
                RadLabel Lid = ((RadLabel)eeditedItem.FindControl("lblLicenceId"));
                if (Lid != null)
                    ViewState["IEMPLICENCEID"] = Lid.Text;
                else if (LidEdit != null)
                    ViewState["IEMPLICENCEID"] = LidEdit.Text;
                else
                    ViewState["IEMPLICENCEID"] = null;
                BindData();
                gvCrewLicence.Rebind();
                BindFEData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewLicence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton att = (LinkButton)item.FindControl("cmdAtt");

            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");
            }
            RadLabel expdate = (RadLabel)item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

            RadLabel lbl = (RadLabel)item.FindControl("lblLicenceId");

            RadLabel lbtn = (RadLabel)item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            UserControlDocuments ucDocuments = (UserControlDocuments)item.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCE"].ToString();

            UserControlCountry ucFlag = (UserControlCountry)item.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedCountry = drv["FLDFLAGID"].ToString();
        }
    }
    protected void gvFlagEndorsement_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindFEData();
    }
    protected void gvFlagEndorsement_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToString().ToUpper() == "VIEW" || e.CommandName.ToString().ToUpper() == "SELECT")
            {   //BindFEData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFlagEndorsement_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel expdate = (RadLabel)item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)item.FindControl("imgFlag");
            LinkButton att = (LinkButton)item.FindControl("cmdAtt");
            if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                att.Controls.Add(html);
            }
            att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");

            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
            RadLabel lbtn = (RadLabel)item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            UserControlCountry ucCountry = (UserControlCountry)item.FindControl("ddlFlagEdit");
            if (ucCountry != null) ucCountry.SelectedCountry = drv["FLDFLAGID"].ToString();


            UserControlDocuments ucDocuments = (UserControlDocuments)item.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCEID"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)item.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();
        }
    }
    protected void gvDCE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDCEData();
    }
    protected void gvDCE_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToString().ToUpper() == "VIEW" || e.CommandName.ToString().ToUpper() == "SELECT")
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvDCE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton att = (LinkButton)item.FindControl("cmdAtt");
            if (drv["FLDISATTACHMENT"].ToString() == string.Empty)
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                att.Controls.Add(html);
            }
            att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&u=n'); return false;");

            RadLabel expdate = (RadLabel)item.FindControl("lblExpiryDate");
            Image imgFlag = (Image)item.FindControl("imgFlag");
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    //e.Row.CssClass = "rowyellow";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    //e.Row.CssClass = "rowred";
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

            RadLabel lbtn = (RadLabel)item.FindControl("lblVerified");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucToolTipLicense");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");



            UserControlDocuments ucDocuments = (UserControlDocuments)item.FindControl("ddlLicenceEdit");
            if (ucDocuments != null) ucDocuments.SelectedDocument = drv["FLDLICENCE"].ToString();

            UserControlFlag ucFlag = (UserControlFlag)item.FindControl("ddlFlagEdit");
            if (ucFlag != null) ucFlag.SelectedFlag = drv["FLDFLAGID"].ToString();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        BindFEData();
        BindDCEData();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "Licence", "Licence No.", "Place of Issue", "Issued", "Expiry", "Licence Issuing Country", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 0, 1
                        , sortexpression, sortdirection
                        , gvCrewLicence.CurrentPageIndex + 1, gvCrewLicence.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewLicence", "National Documents", alCaptions, alColumns, ds);
            gvCrewLicence.DataSource = ds;
            gvCrewLicence.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "1")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Licence", "Licence No. ", "Place of Issue", "Issued", "Expiry", "Licence Issuing Country", "Verified", "Issuing Authority" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 0, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("National Documents", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "2")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "Endorsement", "Endorsement No. ", "Place of Issue", "Issued", "Expiry", "Flag", "Verified", "Issuing Authority" };
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["FESORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());

                int? iEmpLicenceId = null;
                if (ViewState["IEMPLICENCEID"] != null)
                {
                    iEmpLicenceId = General.GetNullableInteger(ViewState["IEMPLICENCEID"].ToString());
                }
                DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentVesselCrewSelection),
                             iEmpLicenceId, 1
                            , sortexpression, sortdirection
                            , 1
                            , General.ShowRecords(null)
                            , ref iRowCount
                            , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Flag Documents", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("EXCEL") && Request.QueryString["e"] == "3")
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
                string[] alCaptions = { "DCE", "DCE No.", "Place of Issue", "Issued", "Expiry", "Flag", "Verified", "Issuing Authority" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                       Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 1, 1
                       , sortexpression, sortdirection
                       , 1
                       , General.ShowRecords(null)
                       , ref iRowCount
                       , ref iTotalPageCount);
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Dangerous Cargo Endorsements", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEmployee.EditVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToInt32(Filter.CurrentVesselCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void BindFEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "Endorsement", "Endorsement No.", "Place of Issue", "Issued", "Expiry", "Flag", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["FESORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["FESORTDIRECTION"].ToString());
            int? iEmpLicenceId = null;
            if (ViewState["IEMPLICENCEID"] != null)
            {
                iEmpLicenceId = General.GetNullableInteger(ViewState["IEMPLICENCEID"].ToString());
            }

            DataSet ds = PhoenixCrewLicenceFlagEndorsement.CrewLicenceFlagEndorsementSearch(General.GetNullableInteger(Filter.CurrentVesselCrewSelection),
                         iEmpLicenceId, 1
                        , sortexpression, sortdirection
                         , gvFlagEndorsement.CurrentPageIndex + 1, gvFlagEndorsement.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvFlagEndorsement", "Flag Documents", alCaptions, alColumns, ds);

            gvFlagEndorsement.DataSource = ds;
            gvFlagEndorsement.VirtualItemCount = iRowCount;
            ViewState["FEROWCOUNT"] = iRowCount;
            ViewState["FETOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDCEData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDLICENCENAME", "FLDLICENCENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDFLAGNAME", "FLDVERIFIEDBYNAME", "FLDISSUEDBY" };
            string[] alCaptions = { "DCE", "DCE No.", "Place of Issue", "Issued", "Expiry", "Flag", "Verified", "Issuing Authority" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewLicence.CrewLicenceSearch(
                        Int32.Parse(Filter.CurrentVesselCrewSelection.ToString()), 1, 1
                        , sortexpression, sortdirection
                        , gvDCE.CurrentPageIndex + 1, gvDCE.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);
            General.SetPrintOptions("gvDCE", "Dangerous Cargo Endorsements", alCaptions, alColumns, ds);

            gvDCE.DataSource = ds;
            gvDCE.VirtualItemCount = iRowCount;
            ViewState["DCEROWCOUNT"] = iRowCount;
            ViewState["DCETOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
