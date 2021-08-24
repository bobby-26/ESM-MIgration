using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
using System.Text;

public partial class CrewOffshoreEmployeeDocumentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeeDocumentList.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeeDocumentList.aspx", "Email", "<i class=\"fas fa-envelope\"></i>", "EMAIL");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["empid"] = Request.QueryString["empid"].ToString();

                if (Request.QueryString["pdform"] != null && Request.QueryString["pdform"].ToString() != "")
                    ViewState["pdform"] = Request.QueryString["pdform"].ToString();

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME" };
                string[] alCaptions = { "Document Name", "Document Number", "Issued Date", "Expiry Date", "Place Of Issue", "Country/Flag" };

                DataSet ds = PhoenixCrewEmployeeDocument.ListCrewDocumentActiveVerified(
                               Int32.Parse(ViewState["empid"].ToString()));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Document List", ds.Tables[0], alColumns, alCaptions, null, null);
            }
            if (CommandName.ToUpper().Equals("EMAIL"))
            {
                GetSelectedDocument();

                //NameValueCollection nvc = Filter.CurrentCrewDocumentListFilter;

                //string strlist = General.GetNullableString(nvc != null ? nvc.Get("DocumentDtkey") : string.Empty);

                //if (!isValidMap(strlist))
                //{
                //    ucError.Visible = true;
                //    return;
                //}

                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewEmployeeDocumentEmail.aspx?pdform=" + ViewState["pdform"] + "',false,1000,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected bool isValidMap(string csvCrewPlanList)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(csvCrewPlanList) == null)
            ucError.ErrorMessage = "Please select any document";

        return (!ucError.IsError);
    }


    protected void GetSelectedDocument()
    {
        try
        {
            Filter.CurrentCrewDocumentListFilter = null;

            StringBuilder strlist = new StringBuilder();
            StringBuilder strtype = new StringBuilder();

            if (gvCrewSearch.MasterTableView.Items.Count > 0)
            {
               
                if (gvCrewSearch.SelectedItems.Count > 0)
                {
                    foreach (GridDataItem gvr in gvCrewSearch.SelectedItems)
                    {
                        RadLabel lblDocumentDtkey = (RadLabel)gvr.FindControl("lblDocumentDtkey");
                        RadLabel lblsubtype = (RadLabel)gvr.FindControl("lblsubtype");

                        strlist.Append(lblDocumentDtkey.Text);
                        strlist.Append(",");

                        strtype.Append(lblsubtype.Text);
                        strtype.Append(",");
                    }

                }
                if (strlist.Length > 1)
                {
                    strlist.Remove(strlist.Length - 1, 1);
                }

                if (strtype.Length > 1)
                {
                    strtype.Remove(strtype.Length - 1, 1);
                }

            }

            NameValueCollection criteria = new NameValueCollection();
            criteria.Add("DocumentDtkey", strlist.ToString());
            criteria.Add("DocumentType", strtype.ToString());
            criteria.Add("EmpId", ViewState["empid"].ToString());

            Filter.CurrentCrewDocumentListFilter = criteria;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {

            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME" };
            string[] alCaptions = { "Document Name", "Document Number", "Issued Date", "Expiry Date", "Place Of Issue", "Country/Flag" };

            DataSet ds = PhoenixCrewEmployeeDocument.ListCrewDocumentActiveVerified(
                        Int32.Parse(ViewState["empid"].ToString()));

            General.SetPrintOptions("gvCrewSearch", "Document List", alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;

        BindData();

    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            RadLabel lblsubtype = (RadLabel)e.Item.FindControl("lblsubtype");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                string mimetype = ".pdf";

                if (lblsubtype.Text == "PASSPORT")
                {
                    att.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD&U=1'); return false;";
                }
                if (lblsubtype.Text == "SEAMANBOOK")
                {
                    att.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD&U=1'); return false;";
                }

                if (lblsubtype.Text == "TRAVEL")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=TRAVELDOCUPLOAD&U=1'); return false;");
                }
                if (lblsubtype.Text == "NATIONAL LICENCE")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                           + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=LICENCEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "COURSE")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "OTHER")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                          + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "MEDICAL TEST")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALTESTUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "MEDICAL")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                         + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }


            }

        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewSearch.Rebind();
    }


}