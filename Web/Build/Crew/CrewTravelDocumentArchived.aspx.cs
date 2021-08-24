using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewTravelDocumentArchived : PhoenixBasePage
{
    string empid = string.Empty;
    string familyid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            empid = Request.QueryString["empid"];
            familyid = Request.QueryString["familyid"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Documents", "DOCUMENTS");

            if (Request.QueryString["familyid"] == null || Request.QueryString["familyid"] == "")
            {
                toolbar.AddButton("Passport", "PASSPORT");
                toolbar.AddButton("Seaman Book", "SEAMANBOOK");
                toolbar.AddButton("US Visa", "USVISA");
                toolbar.AddButton("MCV Australia", "MCVAUSTRALIA");             
            }
            MenuTravelDocument.AccessRights = this.ViewState;
            MenuTravelDocument.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvTravelDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["ODPAGENUMBER"] = 1;
                ViewState["ODSORTEXPRESSION"] = null;
                ViewState["ODSORTDIRECTION"] = null;
                ViewState["ODCURRENTINDEX"] = 1;
                gvOtherDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            if (Request.QueryString["type"] == "1")
            {
                MenuTravelDocument.SelectedMenuIndex = 0;
                gvOtherDocument.Visible = false;
                BindData();

            }
            else if (Request.QueryString["type"] == "2")
            {
                MenuTravelDocument.Visible = false;
                gvTravelDocument.Visible = false;
                BindODData();
            }
            else if (Request.QueryString["type"] == "3")
            {
                if (Request.QueryString["cmdtype"] == "1")
                {
                    MenuTravelDocument.SelectedMenuIndex = 1;
                }
                else if (Request.QueryString["cmdtype"] == "2")
                {
                    MenuTravelDocument.SelectedMenuIndex = 2;
                }
                else if (Request.QueryString["cmdtype"] == "3")
                {
                    MenuTravelDocument.SelectedMenuIndex = 3;
                }
                else if (Request.QueryString["cmdtype"] == "4")
                {
                    MenuTravelDocument.SelectedMenuIndex = 4;
                }
                gvTravelDocument.Visible = false;
                gvOtherDocument.Visible = false;
                BindEmployeeDocument();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PASSPORT"))
            {
                Response.Redirect("CrewTravelDocumentArchived.aspx?empid=" + empid + "&type=3&cmdtype=1&familyid=" + familyid, false);
            }
            else if (CommandName.ToUpper().Equals("SEAMANBOOK"))
            {
                Response.Redirect("CrewTravelDocumentArchived.aspx?empid=" + empid + "&type=3&cmdtype=2&familyid=" + familyid, false);
            }
            else if (CommandName.ToUpper().Equals("USVISA"))
            {
                Response.Redirect("CrewTravelDocumentArchived.aspx?empid=" + empid + "&type=3&cmdtype=3&familyid=" + familyid, false);
            }
            else if (CommandName.ToUpper().Equals("MCVAUSTRALIA"))
            {
                Response.Redirect("CrewTravelDocumentArchived.aspx?empid=" + empid + "&type=3&cmdtype=4&familyid=" + familyid, false);
            }
            if (CommandName.ToUpper().Equals("DOCUMENTS"))
            {
                Response.Redirect("CrewTravelDocumentArchived.aspx?empid=" + empid + "&type=1&familyid=" + familyid, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelDocument.CurrentPageIndex + 1;
        BindData();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeTravelDocument(int.Parse(empid), General.GetNullableInteger(Request.QueryString["familyid"]), 0,
                                                                               sortexpression, sortdirection
                                                                               , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                               , gvTravelDocument.PageSize
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount);


            gvTravelDocument.DataSource = ds;
            gvTravelDocument.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvTravelDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                if (Request.QueryString["t"] == "n")
                    PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 1);
                else
                    PhoenixCrewTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(empid), int.Parse(id), 1);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                BindData();
                gvTravelDocument.Rebind();
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

    protected void gvTravelDocument_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

            PhoenixCrewTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(traveldocumentid)
                                                                );
            BindData();
            gvTravelDocument.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&u=n'); return false;");
            }

            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            if (lbl != null)
            {
                img.Attributes.Add("onclick", "openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&s=n','xlarge')");
            }
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                if (img != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    img.Controls.Add(html);
                }

            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
        }
    }

    protected void gvTravelDocument_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvOtherDocument_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["ODPAGENUMBER"] = ViewState["ODPAGENUMBER"] != null ? ViewState["ODPAGENUMBER"] : gvOtherDocument.CurrentPageIndex + 1;
        BindODData();
    }

    protected void gvOtherDocument_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["ODSORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["ODSORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["ODSORTDIRECTION"] = "1";
                break;
        }

    }


    private void BindODData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["ODSORTEXPRESSION"] == null) ? null : (ViewState["ODSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["ODSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["ODSORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewTravelDocument.SearchEmployeeOtherDocument(Convert.ToInt32(empid), General.GetNullableInteger(Request.QueryString["familyid"]), 0, 0,
                                                                               sortexpression, sortdirection
                                                                                , Int32.Parse(ViewState["ODPAGENUMBER"].ToString())
                                                                               , gvTravelDocument.PageSize
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount);

            gvOtherDocument.DataSource = ds;
            gvOtherDocument.VirtualItemCount = iRowCount;

            ViewState["ODROWCOUNT"] = iRowCount;
            ViewState["ODTOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;
                if (Request.QueryString["t"] == "n")
                    PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 1);
                else
                    PhoenixCrewTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(empid), int.Parse(id), 1);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                BindODData();
                gvOtherDocument.Rebind();
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

    protected void gvOtherDocument_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string traveldocumentid = ((RadLabel)e.Item.FindControl("lbltraveldocumentid")).Text;

            PhoenixCrewTravelDocument.DeleteEmployeeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , Convert.ToInt32(traveldocumentid)
                                                                );
            BindODData();
            gvOtherDocument.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherDocument_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&u=n'); return false;");
            }

            RadLabel lbl = (RadLabel)e.Item.FindControl("lbltraveldocumentid");
            LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
            if (lbl != null)
            {
                if (img != null)
                    img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
            }

            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                if (img != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();                    
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                    img.Controls.Add(html);
                }

            }
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }
        }
    }


    protected void gvEmpDoc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindEmployeeDocument();
    }


    private void BindEmployeeDocument()
    {
        try
        {

            if (Request.QueryString["cmdtype"] != null && Request.QueryString["cmdtype"].ToString() != string.Empty)
            {
                DataTable dt = PhoenixCrewTravelDocument.ListEmployeeDocumentArchive(int.Parse(empid), int.Parse(Request.QueryString["cmdtype"]));

                gvEmpDoc.DataSource = dt;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmpDoc_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
            {
                string id = ((RadLabel)e.Item.FindControl("lblHistoryid")).Text;

                if (Request.QueryString["t"] == "n")
                { PhoenixNewApplicantTravelDocument.ArchiveEmployeeTravelDocument(int.Parse(id), 1); }
                else
                {
                    if (Request.QueryString["cmdtype"] == "1")
                    {
                        PhoenixCrewTravelDocument.EmployeePassportDeArchive(int.Parse(empid), new Guid(id));
                    }
                    else if (Request.QueryString["cmdtype"] == "2")
                    {
                        PhoenixCrewTravelDocument.EmployeeSeamanBookDeArchive(int.Parse(empid), new Guid(id));
                    }
                    else if (Request.QueryString["cmdtype"] == "3")
                    {
                        PhoenixCrewTravelDocument.EmployeeUSVisaDeArchive(int.Parse(empid), new Guid(id));
                    }
                    else if (Request.QueryString["cmdtype"] == "4")
                    {
                        PhoenixCrewTravelDocument.EmployeeMCVAustraliaDeArchive(int.Parse(empid), new Guid(id));
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                BindEmployeeDocument();
                gvEmpDoc.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmpDoc_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string historyid = ((RadLabel)e.Item.FindControl("lblHistoryid")).Text;

            PhoenixCrewTravelDocument.DeleteEmployeeDocumentArchive(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(historyid));
            BindEmployeeDocument();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEmpDoc_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdArchive");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
                if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            RadLabel lblHistoryid = (RadLabel)e.Item.FindControl("lblHistoryid");
            string attachmenttype = "";

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                if (Request.QueryString["cmdtype"] == "1")
                {
                    attachmenttype = PhoenixCrewAttachmentType.PASSPORT.ToString();

                    gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;

                }
                else if (Request.QueryString["cmdtype"] == "2")
                {
                    attachmenttype = PhoenixCrewAttachmentType.SEAMANBOOK.ToString();

                    gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = true;
                    gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;

                }
                else if (Request.QueryString["cmdtype"] == "3")
                {
                    attachmenttype = PhoenixCrewAttachmentType.USVISA.ToString();

                    gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = true;
                    gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;

                }
                else if (Request.QueryString["cmdtype"] == "4")
                {
                    attachmenttype = PhoenixCrewAttachmentType.MCVAUSTRALIA.ToString();
                    gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                    gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = true;

                }
                else
                {
                    attachmenttype = PhoenixCrewAttachmentType.DOCUMENTS.ToString();
                }

                att.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblHistoryid.Text + "&mod="
                    + PhoenixModule.CREW + "&type=" + attachmenttype + "&u=n')");

            }

            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
            RadLabel expdate = e.Item.FindControl("lblDateofExpiry") as RadLabel;
            DateTime? d = General.GetNullableDateTime(expdate.Text);
            if (d.HasValue)
            {
                TimeSpan t = d.Value - DateTime.Now;
                if (t.Days >= 0 && t.Days < 120)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow.png";
                }
                else if (t.Days < 0)
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red.png";
                }
            }

        }

        if (e.Item is GridHeaderItem)
        {
            if (Request.QueryString["cmdtype"] == "1")
            {
                gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;
            }
            else if (Request.QueryString["cmdtype"] == "2")
            {

                gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = true;
                gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;

            }
            else if (Request.QueryString["cmdtype"] == "3")
            {
                gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = true;
                gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = false;

            }
            else if (Request.QueryString["cmdtype"] == "4")
            {
                gvEmpDoc.MasterTableView.GetColumn("NationalityHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("EcnrHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("MinpageHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("TypeHeader").Display = false;
                gvEmpDoc.MasterTableView.GetColumn("RemarksHeader").Display = true;

            }
        }
    }


}
