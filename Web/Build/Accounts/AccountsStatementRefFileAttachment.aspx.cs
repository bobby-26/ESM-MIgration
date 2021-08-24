using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web;
using Telerik.Web.UI;

public partial class AccountsStatementRefFileAttachment : PhoenixBasePage
{
    //const int TimedOutExceptionCode = -2147467259;
    string attachmentcode = string.Empty;
    PhoenixModule module;
   
    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Filter.CurrentMenuCodeSelection != null)
            SessionUtil.PageAccessRights(this.ViewState);

        cmdname = (Request.QueryString["cmdname"] == null ? "UPLOAD" : Request.QueryString["cmdname"].ToUpper());
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]) && !string.IsNullOrEmpty(Request.QueryString["mod"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
            module = (PhoenixModule)(Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]));
        }
        else
        {
            string msg = string.IsNullOrEmpty(Request.QueryString["dtkey"]) ? "Select a record to add attachment" : string.Empty;
            msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
            ucError.ErrorMessage = msg;
            ucError.Visible = true;
        }
        if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
            ViewState["adjustmentamount"] = Request.QueryString["adjustmentamount"];
        if (!IsPostBack)
        {
            if (Request.QueryString["VESSELID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }
            if (Request.QueryString["pdapprovalyn"] != null)
            {
                ddlStatus.Visible = true;
                lblStatus.Visible = true;
            }
            if (Request.QueryString["mimetype"] != null)
            {
                ViewState["AllowedMIMEType"] = Request.QueryString["mimetype"].ToString();
            }

            if (Request.QueryString["debitnotereferenceid"] != null)
            {
                ViewState["debitnotereferenceid"] = Request.QueryString["debitnotereferenceid"].ToString();
            }

            if (Request.QueryString["maxnoofattachments"] != null && Request.QueryString["maxnoofattachments"].ToString() != "")
                ViewState["maxnoofattachments"] = Request.QueryString["maxnoofattachments"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BYVESSEL"] = (string.IsNullOrEmpty(Request.QueryString["BYVESSEL"]) ? string.Empty : Request.QueryString["BYVESSEL"]);
            if (!string.IsNullOrEmpty(Request.QueryString["maxreqlen"]))
            {
                ucError.ErrorMessage = "File is too large to Upload. Maximum Upload Size is " + Math.Round(int.Parse(Request.QueryString["maxreqlen"].ToString()) / 1048576.00 * 100000.00) / 100000 + " MB";
                ucError.Visible = true;
            }

            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        if (string.IsNullOrEmpty(Request.QueryString["U"]) || Request.QueryString["ratingyn"] == "1")
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", cmdname,ToolBarDirection.Right);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.MenuList = toolbarmain.Show();
        }

        if (module.ToString() == "ACCOUNTS" && Request.QueryString["Status"] == "Published")
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", cmdname);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.Visible = false;
        }

        if (module.ToString() == "CREW" && !String.IsNullOrEmpty(attachmentcode))
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageLink("javascript:openNewWindow('download','','"+Session["sitepath"]+"/Common/CommonViewAllAttachments.aspx?dtkey=" + attachmentcode + "&mod=" + module + "&type=" + type + "')", "View All Images", "view_gallery.png", "VIEWALL");
            SubMenuAttachment.AccessRights = this.ViewState;
            SubMenuAttachment.MenuList = toolbargrid.Show();
        }

        //BindData();
        //SetPageNavigator();
    }
    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToString().ToUpper() == cmdname)
        {
			if (!string.IsNullOrEmpty(attachmentcode))
			{
				
					try
					{
						int? vesselid = null;
						string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;

                        if (ViewState["maxnoofattachments"] != null && ViewState["maxnoofattachments"].ToString() != "")
                        {
                            if (ViewState["ROWCOUNT"].ToString().Equals(ViewState["maxnoofattachments"]))
                            {
                                ucError.ErrorMessage = "You cannot upload more than " + ViewState["maxnoofattachments"] + " attachments.";
                                ucError.Visible = true;
                                return;
                            }
                        }

						if(Request.QueryString["pdapprovalyn"] != null)
						{
							if (General.GetNullableInteger(ddlStatus.SelectedHard) != null)
							{
								if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
								{
									PhoenixCommonCrew.CrewApprovedPDInsert(
										PhoenixSecurityContext.CurrentSecurityContext.UserCode,
										attachmentcode,
										type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? 0 : 1,
										General.GetNullableInteger(ddlStatus.SelectedHard));

									string atttype = "";
									atttype = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
									PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(atttype.ToString()), VesselId());

									ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
											 "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

									BindData();
									gvAttachment.Rebind();

									return;
								}
							}
							else
							{
								string msg = "Select a status for the attachment";
								ucError.ErrorMessage = msg;
								ucError.Visible = true;
							}
						}

						if (Request.QueryString["mod"].ToString().ToUpper() == "ACCOUNTS")
						{
							vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                            HttpFileCollection file = Request.Files;
                            HttpPostedFile filename = file.Get(0);
                            string fname = filename.FileName;

                            DataTable dt = new DataTable();
                            dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(attachmentcode), General.GetNullableString(type));
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (fname == dt.Rows[i]["FLDFILENAME"].ToString())
                                {
                                    ucError.ErrorMessage = "Filename alredy exist.";
                                    ucError.Visible = true;
                                    return;
                                }
                            }

                            if (ViewState["AllowedMIMEType"] != null)
                            {
                                PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module,
                                    null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                            }
                            else
                            {
                                PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
									 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

							PhoenixCommonAcount.UpdateStatementReference(new Guid(attachmentcode));
							ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
							BindData();
                        gvAttachment.Rebind();
                        return;
						}

                        if (Request.QueryString["mod"].ToString() == "QUALITY" && (type.ToUpper() == "INSPECTIONREPORT" || type.ToUpper() == "OPERATORCOMMENTS" || type.ToUpper() == "APPROVALLETTER"))
                        {
                            vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                            BindData();
                        gvAttachment.Rebind();

                        return;
                        }

                        if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == "APPRAISAL"))
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                            PhoenixCommonCrew.CrewAppraisalFinaliseByAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                            BindData();
                        gvAttachment.Rebind();

                        return;
                        }


						vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                        if (ViewState["AllowedMIMEType"] != null)
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module,
                                null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                        }
                        else
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        }
                    
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
								     "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

					}
					catch (Exception ex)
					{
						ucError.ErrorMessage = ex.Message;
						ucError.Visible = true;
					}
					BindData();
					gvAttachment.Rebind();
			}
			else
			{
				string msg = "Select a record to add attachment";
				msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
				ucError.ErrorMessage = msg;
				ucError.Visible = true;
			}

        }

    }


    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (attachmentcode != string.Empty)
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
            {
                type = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
            }

            if (ViewState["BYVESSEL"].ToString() == string.Empty)
            {
                ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                             gvAttachment.PageSize,
                                                                            ref iRowCount, ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type
                                                                          , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                          ,sortexpression, sortdirection,
                                                                          Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                          gvAttachment.PageSize,
                                                                          ref iRowCount, ref iTotalPageCount);
            }
        }

        gvAttachment.DataSource = ds;
        gvAttachment.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["ondblclick"] = "";
    }

    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            ((CheckBox)_gridView.Rows[e.NewEditIndex].FindControl("chkSynch")).Focus();
            gvAttachment.Rebind();
            RadLabel lblFileName = ((RadLabel)_gridView.Rows[e.NewEditIndex].FindControl("lblFileName"));
            Image imgtype = (Image)_gridView.Rows[e.NewEditIndex].FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
            case ".docx":
                imagepath += "word.png";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }


    private int? VesselId()
    {
        int? VesselId = 0;
        string menucode = Filter.CurrentMenuCodeSelection;
        if (menucode == "REG-GEN-VLM" || menucode.StartsWith("INV") || menucode.StartsWith("PMS")
            || menucode.StartsWith("PUR") || menucode.StartsWith("CRW-PBL") || menucode.StartsWith("VAC"))
        {
            if (menucode == "REG-GEN-VLM")
                VesselId = General.GetNullableInteger(Filter.CurrentVesselMasterFilter != null ? Filter.CurrentVesselMasterFilter : "0");
            else
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        return VesselId;
    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
            bool chk = ((CheckBox)e.Item.FindControl("chkSynch")).Checked;
            PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
  
        BindData();
        gvAttachment.Rebind();
    }

    protected void gvAttachment_DeleteCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
            if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
            {
                if (Convert.ToDouble(ViewState["adjustmentamount"]) == 0.00)
                {
                    string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                    String path = MapPath("~/attachments/" + lblFilePath.Text);
                    System.IO.File.Delete(path);
                }
                else
                {
                    ucError.ErrorMessage = "Not possible to delete the Attachment after adjustment amount as modified";
                    ucError.Visible = true;
                }
            }

            if (Request.QueryString["MOD"].ToString().ToUpper() == "ACCOUNTS")
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                if (General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()) != null)
                    PhoenixAccountsSOAGeneration.AttachmentVerificationDetailsDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()), new Guid(dtkey));
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                String path = MapPath("~/attachments/" + lblFilePath.Text);
                System.IO.File.Delete(path);
                PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
            }
            else
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                String path = MapPath("~/attachments/" + lblFilePath.Text);
                System.IO.File.Delete(path);

                if (Request.QueryString["MOD"].ToString().ToUpper() == "OFFSHORE")
                {
                    PhoenixCommonFileAttachment.CBTCrewAuditAttachmentDelete(new Guid(dtkey));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        gvAttachment.Rebind();
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                if (Request.QueryString["u"] != null)
                {
                    db.Visible = false;
                    ed.Visible = false;
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }
                if (module.ToString() == "ACCOUNTS" && Request.QueryString["Status"] == "Published")
                {
                    db.Visible = false;
                    ed.Visible = false;
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }

                RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                Image imgtype = (Image)e.Item.FindControl("imgfiletype");
                if (lblFileName.Text != string.Empty)
                {
                    imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                    RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                    lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                    //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                    //lnk.Style.Add("text-decoration", "underline");
                    //lnk.Style.Add("cursor", "pointer");
                    //lnk.Style.Add("color", "blue");
                    //lnk.Attributes.Add("onclick", "javascript:Openpopup('download','','" + Session["sitepath"] + "/attachments/" + lblFilePath.Text + "')");
                }
            }
        }
       
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
