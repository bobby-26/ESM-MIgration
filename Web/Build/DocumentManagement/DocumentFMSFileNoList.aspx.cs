using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class DocumentFMSFileNoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbarm = new PhoenixToolbar();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarm.AddButton("E Mails", "ESMA", ToolBarDirection.Left);
                toolbarm.AddButton("ESM Filing", "ESMF", ToolBarDirection.Left);
            }
            toolbarm.AddButton("Shipboard Forms", "SPFF", ToolBarDirection.Left);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbarm.AddButton("Office Forms", "OFFF", ToolBarDirection.Left);

            toolbarm.AddButton("Maintenance Forms", "MCFS", ToolBarDirection.Left);
            toolbarm.AddButton("Plans and Drawings", "DRWS", ToolBarDirection.Left);
            toolbarm.AddButton("Manuals", "MNSF", ToolBarDirection.Left);
            toolbarm.AddButton("Equipment Manuals", "EQSF", ToolBarDirection.Left);
            MenuFMS.AccessRights = this.ViewState;
            MenuFMS.MenuList = toolbarm.Show();
            MenuFMS.SelectedMenuIndex = 1;

            PhoenixToolbar toolbarmail = new PhoenixToolbar();
            toolbarmail.AddButton("File Number", "FILEN", ToolBarDirection.Right);
            toolbarmail.AddButton("Inbox", "INB", ToolBarDirection.Right);

            //MenuFMSmail.MenuList = toolbarmail.Show();
            //MenuFMSmail.SelectedMenuIndex = 0;

            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentFMSFileNoList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFMSFileNo')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentFMSFileNoList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','FMS','DocumentManagement/DocumentManagementFMSFileNoUpload.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentFMSFileNoList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersFMSFileNo.AccessRights = this.ViewState;
            MenuRegistersFMSFileNo.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvFMSFileNo.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ddlsource.DataSource = PhoenixRegisterFMSMail.FMSTypeList();
                ddlsource.DataTextField = "FLDFMSTYPENAME";
                ddlsource.DataValueField = "FLDFMSTYPEID";
                ddlsource.DataBind();

                ViewState["DATE"] = "";
                DateTime now = DateTime.Now;
                ViewState["DATE"] = now.Date.AddYears(-5).ToShortDateString();

                ViewState["TODATE"] = now.Date.AddMonths(6).ToShortDateString();

                ddlvessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
            ViewState["VESSELID"] = ddlvessel.SelectedVessel;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFMSmail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FILEN"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("INB"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx?", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFMS_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("ESMA"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMailList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("ESMF"))
            {
                Response.Redirect("../DocumentManagement/DocumentFMSFileNoList.aspx", false);
            }
            if (CommandName.ToUpper().Equals("SPFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSShipboardFormList.aspx?CATEGORYNO=2", false);
            }
            if (CommandName.ToUpper().Equals("OFFF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSOfficeFormList.aspx?CATEGORYNO=16&Callfrom=1", false);
            }
            if (CommandName.ToUpper().Equals("MCFS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSMaintenanceHistoryTemplate.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("DRWS"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSDrawingList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("MNSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSVesselSurveyScheduleList.aspx?", false);
            }
            if (CommandName.ToUpper().Equals("EQSF"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFMSEquipmentManuals.aspx?", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSFileNo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlsource_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
        gvFMSFileNo.Rebind();
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFILENO", "FLDFILENODESCRIPTION", "FLDFILENOHINT", "FLDSOURCETYPE" };
            string[] alCaptions = { "File No", "Description", "Hint", "Active Y/N" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string fileno = (txtFileNoSearch.Text == null) ? "" : txtFileNoSearch.Text;

            ds = PhoenixRegisterFMSMail.FMSFileNoSearch(
                fileno,
                txtDescription.Text,
                null,
                General.GetNullableInteger(ddlsource.SelectedValue),
                sortexpression,
                sortdirection,
                gvFMSFileNo.CurrentPageIndex + 1,
                //gvFMSFileNo.PageSize,
                int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                ref iRowCount,
                ref iTotalPageCount
                );

            Response.AddHeader("Content-Disposition", "attachment; filename=FMSFileNo.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Company</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersFMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvFMSFileNo.SelectedIndexes.Clear();
                //gvFMSFileNo.EditIndexes.Clear();
                //gvFMSFileNo.DataSource = null;
                gvFMSFileNo.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDescription.Text = "";
                txtFileNoSearch.Text = "";
                gvFMSFileNo.SelectedIndexes.Clear();
                gvFMSFileNo.EditIndexes.Clear();
                gvFMSFileNo.DataSource = null;
                gvFMSFileNo.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFMSFileNo.Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFILENO", "FLDFILENODESCRIPTION", "FLDFILENOHINT", "FLDSOURCETYPE" };
            string[] alCaptions = { "File No", "Description", "Hint", "Active Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string fileno = (txtFileNoSearch.Text == null) ? "" : txtFileNoSearch.Text;

            DataSet ds = PhoenixRegisterFMSMail.FMSFileNoSearch(
                                                             General.GetNullableString(fileno),
                                                              General.GetNullableString(txtDescription.Text),
                                                             null,
                                                             General.GetNullableInteger(ddlsource.SelectedValue),
                                                             sortexpression,
                                                             sortdirection,
                                                             gvFMSFileNo.CurrentPageIndex + 1,
                                                              gvFMSFileNo.PageSize,
                                                             ref iRowCount,
                                                             ref iTotalPageCount
                                                             );

            General.SetPrintOptions("gvFMSFileNo", "FileNo List", alCaptions, alColumns, ds);

            gvFMSFileNo.DataSource = ds;
            gvFMSFileNo.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSFileNo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string uid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDFMSMAILFILENOID"].ToString();
                PhoenixRegisterFMSMail.DeleteFMSFileno(new Guid(uid));
                gvFMSFileNo.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        int vesselid = 0;
        if (General.GetNullableInteger(ddlvessel.SelectedVessel) != null)
        {
            vesselid = int.Parse(ddlvessel.SelectedVessel);
        }
        else
        {
            ddlvessel.SelectedVessel = "0";
            ViewState["VESSELID"] = "0";
        }

        gvFMSFileNo.SelectedIndexes.Clear();
        gvFMSFileNo.EditIndexes.Clear();
        gvFMSFileNo.DataSource = null;
        gvFMSFileNo.Rebind();


    }

    protected void gvFMSFileNo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel fileid = (RadLabel)e.Item.FindControl("lblFileNo");
                RadLabel filenumberid = (RadLabel)e.Item.FindControl("lblFileNoId");
                RadLabel attach = (RadLabel)e.Item.FindControl("lblisattachment");
                RadLabel source = (RadLabel)e.Item.FindControl("lbltype");
                RadLabel attachmentcode = (RadLabel)e.Item.FindControl("lblattachmentcode");
                LinkButton cmdviewAttachment = (LinkButton)e.Item.FindControl("lblFileNoDescription");
                RadLabel lbltypeid = (RadLabel)e.Item.FindControl("lbltypeid");

                RadLabel urltype = (RadLabel)e.Item.FindControl("lblurltype");
                RadLabel urlfilter = (RadLabel)e.Item.FindControl("lblurlfilter");

                if (attachmentcode != null)
                {
                    string url = string.Empty;
                    cmdviewAttachment.Visible = true;
                    PhoenixRegisterFMSMail.FMSFileNoURLSearch(fileid.Text, ref url);

                    if (lbltypeid.Text == "4") //PHOENIXURL
                    {
                        if (General.GetNullableString(url) != null)
                        {
                            cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "VESSELID=" + ddlvessel.SelectedValue + "', 'false'); false;");
                            //cmdviewAttachment.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + url + "?filenoid=" + filenumberid.Text + "'); return false;";
                        }

                        if (urltype.Text == "0")
                        {
                            NameValueCollection criteria = new NameValueCollection();
                            criteria.Clear();
                            criteria.Add("txtDoneFrom", ViewState["DATE"].ToString());
                            criteria.Add("ucVessel", ViewState["VESSELID"].ToString());
                            Filter.CurrentAuditScheduleFilterCriteria = criteria;

                            if (General.GetNullableString(url) != null)
                            {
                                cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "', 'false'); false;");
                            }
                        }
                        if (urltype.Text == "1")
                        {
                            DataSet ds = PhoenixRegisterFMSMail.FileNoTreeComponentList(ddlvessel.SelectedValue, urlfilter.Text);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string component = "";
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    component = component + "," + dr["FLDCOMPONENTID"].ToString();
                                }
                                ViewState["FMSCOMPONENTID"] = component.Trim().TrimStart(',');
                                url = url + "FromFMS=1&FMSCOMPONENTID=" + ViewState["FMSCOMPONENTID"].ToString();
                                cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "&VESSELID=" + ddlvessel.SelectedValue + "', 'false', '1220px', '620px'); false;");
                            }
                        }
                        if (urltype.Text == "2")
                        {
                            Filter.CurrentDeficiencyFilter = null;
                            NameValueCollection criteria = new NameValueCollection();


                            criteria.Clear();

                            criteria.Add("txtRefNo", string.Empty);
                            criteria.Add("txtFromDate", ViewState["DATE"].ToString());
                            criteria.Add("txtToDate", ViewState["TODATE"].ToString());
                            criteria.Add("ucStatus", string.Empty);
                            criteria.Add("ucVessel", ViewState["VESSELID"].ToString());
                            criteria.Add("ucDefeciencyType", string.Empty);
                            criteria.Add("txtSourceRefNo", string.Empty);
                            criteria.Add("chkRCAReqd", string.Empty);
                            criteria.Add("chkRCACompleted", string.Empty);
                            criteria.Add("chkRCAPending", string.Empty);
                            criteria.Add("ucVesselType", string.Empty);
                            criteria.Add("ucAddressType", string.Empty);
                            criteria.Add("ucCategory", string.Empty);
                            criteria.Add("ucFleet", string.Empty);
                            criteria.Add("ucInspectionType", string.Empty);
                            criteria.Add("ucInspectionCategory", string.Empty);
                            criteria.Add("ucInspection", string.Empty);
                            criteria.Add("ucChapter", string.Empty);
                            criteria.Add("txtKey", string.Empty);
                            criteria.Add("ddlSource", string.Empty);
                            criteria.Add("chkOfficeAuditDeficiencies", "0");
                            criteria.Add("ucCompany", string.Empty);

                            Filter.CurrentSelectedDeficiency = null;
                            Filter.CurrentDeficiencyFilter = criteria;

                            if (General.GetNullableString(url) != null)
                            {
                                cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "', 'false'); false;");
                            }
                        }
                        if (urltype.Text == "3")
                        {
                            NameValueCollection criteria = new NameValueCollection();
                            criteria.Clear();
                            criteria.Add("txtFrom", ViewState["DATE"].ToString());
                            criteria.Add("ucVessel", ViewState["VESSELID"].ToString());
                            Filter.CurrentInspectionScheduleFilterCriteria = criteria;

                            if (General.GetNullableString(url) != null)
                            {
                                cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "', 'false'); false;");
                            }
                        }
                        if (urltype.Text == "4")
                        {
                            if (General.GetNullableString(url) != null)
                            {
                                cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "', 'false'); false;");
                                //cmdviewAttachment.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + url + "?filenoid=" + filenumberid.Text + "'); return false;";
                            }
                        }
                        if (urltype.Text == "5")
                        {
                            if (urlfilter.Text == "1")
                            {
                                if (General.GetNullableString(url) != null)
                                {
                                    cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "FMS=1&STATUS=53&FILTER=5&vslid=" + ddlvessel.SelectedValue + "', 'false'); false;");
                                }
                            }
                            if (urlfilter.Text == "0")
                            {
                                if (General.GetNullableString(url) != null)
                                {
                                    cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "FMS=1&STATUS=53&FILTER=6&vslid=" + ddlvessel.SelectedValue + "', 'false'); false;");
                                }
                            }
                        }
                        if (urltype.Text == "6")
                        {
                            if (urlfilter.Text == "1")
                            {
                                if (General.GetNullableString(url) != null)
                                {
                                    cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "FMS=1&STATUS=55&FILTER=5&vslid=" + ddlvessel.SelectedValue + "', 'false'); false;");
                                }
                            }
                            if (urlfilter.Text == "0")
                            {
                                if (General.GetNullableString(url) != null)
                                {
                                    cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "FMS=1&STATUS=55&FILTER=6&vslid=" + ddlvessel.SelectedValue + "', 'false'); false;");
                                }
                            }
                        }
                    }
                    if ((lbltypeid.Text == "2") || (lbltypeid.Text == "3")) //Electronic Forms
                    {
                        if (urltype.Text == "4")
                        {
                            if (General.GetNullableString(url) != null)
                            {
                                //cmdviewAttachment.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + url + "', 'false'); false;");
                                cmdviewAttachment.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + url + "?filenoid=" + filenumberid.Text + "'); return false;";
                            }
                        }
                    }
                    if (lbltypeid.Text == "1") //UPLOAD
                    {
                        //cmdviewAttachment.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + attachmentcode.Text + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&type=FMSFILENO" + "&BYVESSEL=true&cmdname=FMSFILENOUPLOAD&VESSELID=" + ViewState["VESSELID"].ToString() + "'); return false;";
                        //cmdviewAttachment.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/DocumentManagement/DocumentFMSFileNoAttachment.aspx?filenoid=" + filenumberid.Text + "'); return false;";
                        cmdviewAttachment.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('upload','','{0}/DocumentManagement/DocumentFMSFileNoAttachment.aspx?filenoid=" + filenumberid.Text + "&vesselid=" + ddlvessel.SelectedValue + " ');", Session["sitepath"]));

                    }


                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
