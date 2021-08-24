using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersVesselCertificates : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselCertificates.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselCertificates')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('expired','','Registers/RegistersVesselCertificatesArchived.aspx?vesselid=" + Filter.CurrentVesselMasterFilter + "'); return false;", "Show Archived", "<i class=\"fas fa-envelope-open-text\"></i>", "ARCHIVED");
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselCertificates.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselCertificates.aspx", "Show New Certificates", "<i class=\"fas fa-align-justify\"></i>", "SHOW");
        MenuRegistersVesselCertificates.AccessRights = this.ViewState;
        MenuRegistersVesselCertificates.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            AutoArchive();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvVesselCertificates.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void AutoArchive()
    {
        PhoenixRegistersVesselCertificates.AutoArchiveVesselCertificates(Int16.Parse(Filter.CurrentVesselMasterFilter));
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDREMARKS" };
        string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date", "Authority", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVesselCertificates.VesselCertificatesSearch(Int16.Parse(Filter.CurrentVesselMasterFilter), txtCertificateName.Text, "", null, null, null, "", 1, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselCertificates.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Certificates</h3></td>");
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
    protected void Rebind()
    {
        gvVesselCertificates.SelectedIndexes.Clear();
        gvVesselCertificates.EditIndexes.Clear();
        gvVesselCertificates.DataSource = null;
        gvVesselCertificates.Rebind();
    }
    protected void RegistersVesselCertificates_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        else if (CommandName.ToUpper().Equals("SHOW"))
        {
            Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDREMARKS" };
        string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date", "Authority", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
        DataRow drVessel = dsVessel.Tables[0].Rows[0];
        txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        DataSet ds = PhoenixRegistersVesselCertificates.VesselCertificatesSearch(Int16.Parse(Filter.CurrentVesselMasterFilter), txtCertificateName.Text, "", null, null, null, "", 1, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvVesselCertificates.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvVesselCertificates", "Certificates", alCaptions, alColumns, ds);
        gvVesselCertificates.DataSource = ds;
        gvVesselCertificates.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvVesselCertificates_ItemDataBound(Object sender, GridItemEventArgs e)
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
            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ucCertificateEdit");
            if (ddl != null)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;
                ddl.Items.Clear();
                ddl.DataSource = PhoenixRegistersCertificates.ListCertificates(1, 0);
                ddl.DataTextField = "FLDCERTIFICATENAME";
                ddl.DataValueField = "FLDCERTIFICATEID";
                ddl.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                ddl.DataBind();
                ddl.SelectedValue = dr["FLDCERTIFICATESID"].ToString();
            }

            UserControlAddressType ucAddressType = (UserControlAddressType)e.Item.FindControl("ucIssuingAuthorityEdit");
            DataRowView drview = (DataRowView)e.Item.DataItem;
            if (ucAddressType != null) ucAddressType.SelectedAddress = drview["FLDISSUINGAUTHORITY"].ToString();
            RadLabel expdate = e.Item.FindControl("lblDateOfExpiry") as RadLabel;
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            if (expdate != null)
            {
                DateTime? d = General.GetNullableDateTime(expdate.Text);
                if (d.HasValue)
                {
                    TimeSpan t = d.Value - DateTime.Now;
                    if (t.Days >= 31 && t.Days < 60)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                    }
                    else if (t.Days >= 0 && t.Days <= 30)
                    {
                        //e.Row.CssClass = "rowyellow";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/purple.png";
                    }
                    else if (t.Days < 0)
                    {
                        //e.Row.CssClass = "rowred";
                        imgFlag.Visible = true;
                        imgFlag.ImageUrl = Session["images"] + "/red.png";
                    }
                }
            }
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAtt");
            if (add != null)
            {
                add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
            }
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCertificateName");
            if (lb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.CommandName = "";
            }

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
              + PhoenixModule.REGISTERS + "'); return false;");
            }

        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            RadDropDownList ddl = (RadDropDownList)e.Item.FindControl("ucCertificateAdd");
            if (ddl != null)
            {
                ddl.Items.Clear();
                ddl.DataSource = PhoenixRegistersCertificates.ListCertificates(1, 0);
                ddl.DataTextField = "FLDCERTIFICATENAME";
                ddl.DataValueField = "FLDCERTIFICATEID";
                ddl.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
                ddl.DataBind();
            }
        }
    }
    protected void gvVesselCertificates_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                InsertVesselCertificates(
                    Filter.CurrentVesselMasterFilter
                    , General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ucCertificateAdd")).SelectedValue)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCertificateNumberAdd")).Text)
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateOfIssueAdd")).Text)
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateOfExpiryAdd")).Text)
                    , General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ucIssuingAuthorityAdd")).SelectedAddress)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateVesselCertificates(
                    Filter.CurrentVesselMasterFilter
                    , General.GetNullableInteger(((RadDropDownList)e.Item.FindControl("ucCertificateEdit")).SelectedValue)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCertificateNumberEdit")).Text)
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateOfIssueEdit")).Text)
                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDateOfExpiryEdit")).Text)
                    , General.GetNullableInteger(((UserControlAddressType)e.Item.FindControl("ucIssuingAuthorityEdit")).SelectedAddress)
                    , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDTKeyedit")).Text));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteVesselCertificates(Int16.Parse(Filter.CurrentVesselMasterFilter), Int32.Parse(((RadLabel)e.Item.FindControl("lblCertificatesId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ARCHIVE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixRegistersVesselCertificates.ArchiveVesselCertificates(Int16.Parse(Filter.CurrentVesselMasterFilter), dtkey, 0);
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
    protected void gvVesselCertificates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselCertificates.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    public void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void InsertVesselCertificates(string vesselid, int? certificatesid, string certificatesno, DateTime? dateofissue
        , DateTime? dateofexpiry, int? issuingauthority, string remarks)
    {
        if (!IsValidVesselCertificates(certificatesid, certificatesno, dateofissue, dateofexpiry, issuingauthority, remarks))
        {
            ucError.Visible = true;
            return;
        }
        else
        {
            PhoenixRegistersVesselCertificates.InsertVesselCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Convert.ToInt16(vesselid), Convert.ToInt16(certificatesid), certificatesno, dateofissue
                , dateofexpiry, Convert.ToInt16(issuingauthority), remarks);
        }
    }
    private void UpdateVesselCertificates(string vesselid, int? certificatesid, string certificatesno, DateTime? dateofissue
        , DateTime? dateofexpiry, int? issuingauthority, string remarks, Guid? dtkey)
    {
        if (!IsValidVesselCertificates(certificatesid, certificatesno, dateofissue, dateofexpiry, issuingauthority, remarks))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersVesselCertificates.UpdateVesselCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , int.Parse(vesselid), certificatesid, certificatesno, dateofissue, dateofexpiry, issuingauthority, remarks, dtkey);
        ucStatus.Text = "Vessel Certificate information updated successfully";
    }
    private bool IsValidVesselCertificates(int? certificatesid, string certificateno, DateTime? dateofissue, DateTime? dateofexpiry
        , int? issuingauthority, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (certificatesid.ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Certificate name is required.";

        if (certificateno.Trim().Equals(""))
            ucError.ErrorMessage = "Certificate Number is required.";

        if (dateofissue == null)
            ucError.ErrorMessage = "Valid Date of Issue is required.";

        if (dateofexpiry == null)
            ucError.ErrorMessage = "Valid Date of Expiry is required.";

        if (issuingauthority == null)
            ucError.ErrorMessage = "Issuing Authority is required.";

        if (dateofissue != null && dateofexpiry != null)
        {

            if (dateofissue >= dateofexpiry)
                ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }
    private void DeleteVesselCertificates(int vesselid, int certificatesid)
    {
        PhoenixRegistersVesselCertificates.DeleteVesselCertificates(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, certificatesid);
    }
}