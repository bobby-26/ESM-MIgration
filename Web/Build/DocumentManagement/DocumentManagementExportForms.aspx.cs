using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using SouthNests.Phoenix.StandardForm;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementExportForms : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["STATUS"] = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    ViewState["STATUS"] = Request.QueryString["s"];
                }
                //if (ViewState["STATUS"].ToString() == "INUSE")
                //{
                //    Response.Redirect("../Dashboard/DashboardTechnicalDMFForms.aspx?s=INUSEPDA);
                //}

                //if (ViewState["STATUS"].ToString() == "INUSEPDA")
                //{
                //    RadUpload1.Visible = true;
                //}
                //else
                //    RadUpload1.Visible = false;

            }
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //if (ViewState["STATUS"].ToString() == "INUSE")
            //{
            toolbarmain.AddButton("Export", "EXPORT", ToolBarDirection.Right);
            toolbarmain.AddButton("Import", "IMPORT", ToolBarDirection.Right);
            RadUpload1.Visible = true;
            //}
            //toolbarmain.AddButton("Export", "EXPORT", ToolBarDirection.Right);
            //else if (ViewState["STATUS"].ToString() == "INUSE")
            //{
            //    toolbarmain.AddButton("Import", "IMPORT", ToolBarDirection.Right);
            //}
            ExportForms.AccessRights = this.ViewState;
            ExportForms.MenuList = toolbarmain.Show();
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(ExportForms);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuExportForms_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        //if (ViewState["STATUS"].ToString() == "INUSE")
        //{
        if (CommandName.ToUpper().Equals("EXPORT"))
        {
            string csvjobList = GetSelectedFormsList();
            if (csvjobList.Trim().Equals(""))
            {
                ucError.ErrorMessage = "Select atleast one Form";
                ucError.Visible = true;
                return;
            }
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            ds = PhoenixDashboardTechnical.FormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1, null, null, 1, null, null, 1, 10000, ref iRowCount, ref iTotalPageCount, csvjobList);
            if (ds.Tables[0].Rows.Count > 0)
            {

                string data = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
                Response.Clear();
                Response.ClearHeaders();
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.ContentType = "text/plain";
                Response.AppendHeader("content-disposition", "attachment;filename=\"FORM_" + DateTime.Now.ToShortDateString() + ".txt\"");

                Response.Write(data.ToString());
                Response.End();
            }
        }
        //}
        else if (CommandName.ToUpper().Equals("IMPORT"))
        {
            if (Request.Files.Count > 0)
            {
                foreach (UploadedFile postedFile in RadUpload1.UploadedFiles)
                {
                    using (StreamReader reader = new StreamReader(postedFile.InputStream))
                    {

                        string content = reader.ReadToEnd();
                        content = content.Replace("&", "&amp;");
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        List<dynamic> jsonObjs = js.Deserialize<List<dynamic>>(content);

                        foreach (var item in jsonObjs)
                        {
                            Guid reportIdNew = Guid.Empty;
                            Guid? revisionId = General.GetNullableGuid("");
                            // Guid? ReportId = null;
                            reportIdNew = new Guid(item["FLDREPORTID"]);
                            if (item["FLDFORMREVISIONID"] != "" && item["FLDFORMREVISIONID"] != null)
                                revisionId = new Guid(item["FLDFORMREVISIONID"].Value);
                            try
                            {
                                Guid formId = new Guid(item["FLDFORMID"]);
                                PhoenixFormBuilder.ReportInsert(formId,
                                    General.GetNullableGuid(item["FLDREPORTID"]),
                                    item["FLDOUTPUTJSONSCHEMA"].ToString(),
                                    Convert.ToInt32(item["FLDSTATUS"]),
                                    ref reportIdNew,
                                   PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                    null,
                                    null,
                                    revisionId);
                                ucStatus.Text = "Imported Successfully";
                                //
                            }
                            catch (Exception ex)
                            {
                                ucError.ErrorMessage = ex.Message;
                                ucError.Visible = true;
                            }
                        }

                    }
                }
            }
        }
    }

    //public string SubmitFormData(int rowusercode, FormInput input)
    //{
    //    string dataHD = string.Empty;
    //    string dataDT = string.Empty;
    //    string error = string.Empty;
    //    try
    //    {

    //        string inputjson = JsonConvert.SerializeObject(input);
    //        inputjson = inputjson.Replace("&", "&amp;");
    //        JavaScriptSerializer js = new JavaScriptSerializer();
    //        FormInput stringjson = js.Deserialize<FormInput>(inputjson);
    //        List<FormResponse> forms = new List<FormResponse>();
    //        forms = stringjson.Forms;
    //        if (forms.Count > 0)
    //        {
    //            foreach (var item in forms)
    //            {
    //                Guid? ReportId = null;
    //                ReportId = new Guid(item.RPTId);
    //                try
    //                {
    //                    FormMethods.ImportFormData(rowusercode, new Guid(item.FBId), ReportId, item.FBjson, item.status, item.vesselId);
    //                }
    //                catch (Exception e)
    //                {
    //                    error += e.Message;
    //                    continue;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //dataDT = string.Empty;
    //        error = ex.Message;
    //    }
    //    //result.dataHD = dataHD;
    //    //result.dataDT = dataDT;
    //    //result.error = error;
    //    return error;
    //    //return Json(result, JsonRequestBehavior.AllowGet);
    //}


    protected void txtFileUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {

    }
    public static string ConvertIntoJson(DataSet ds)
    {
        var jsonString = new StringBuilder();
        jsonString.Append("{");
        for (int i = 0; i < ds.Tables.Count; i++)
        {
            jsonString.Append("\"" + ds.Tables[i].TableName + "\":");
            if (ds.Tables[i].TableName == "4")
            {
                jsonString.Append("[]");
            }
            else
            {
                jsonString.Append(ConvertIntoJson(ds.Tables[i]));
            }

            if (i < ds.Tables.Count - 1)
                jsonString.Append(",");
        }
        jsonString.Append("}");
        return jsonString.ToString();
    }
    public static string ConvertIntoJson(DataTable dt)
    {
        var jsonString = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            if (dt.Columns[0].ColumnName == "1")
            {
                jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.TableName == "3" && dt.Columns[j].ColumnName == "4")
                        {
                            string s = ConvertDataTableB32JSON(dt.Rows[i][j].ToString());

                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":"
                            + s + (j < dt.Columns.Count - 1 ? "," : ""));
                        }
                        else
                        {
                            jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":\""
                            + dt.Rows[i][j].ToString().Replace('"', '\"') + (j < dt.Columns.Count - 1 ? "\"," : "\""));
                        }

                    }


                    jsonString.Append(i < dt.Rows.Count - 1 ? "}," : "}");
                }
                return jsonString.Append("]").ToString();
            }
            else
            {
                //jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                        jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":\""
                            + dt.Rows[i][j].ToString().Replace('"', '\"') + (j < dt.Columns.Count - 1 ? "\"," : "\""));

                    jsonString.Append(i < dt.Rows.Count - 1 ? "}," : "}");
                }
                //return jsonString.Append("]").ToString();
                return jsonString.ToString();
            }

        }
        else
        {
            return "[]";
        }
    }
    public static string ConvertDataTableB32JSON(string s)
    {
        String[] strArr = s.Split(',');

        //return strArr.ToString();

        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();


        var jsonString = new StringBuilder();
        jsonString.Append("[");
        for (int i = 0; i < strArr.Length; i++)
        {
            jsonString.Append(i == strArr.Length - 1 ? "\"" + strArr[i] + "\"" : "\"" + strArr[i] + "\",");
        }

        jsonString.Append("]");
        return jsonString.ToString();
    }
    private string GetSelectedFormsList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvForms.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvForms.SelectedItems)
            {
                RadLabel lblReportId = (RadLabel)gv.FindControl("lblReportId");
                strlist.Append(lblReportId.Text + ",");
            }
        }
        return strlist.ToString();
    }
    protected void gvForms_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["STATUS"].ToString() == "INUSE")
        {
            gvForms.MasterTableView.Columns.FindByUniqueName("ClientSelectColumn").Visible = true;
        }
        else
            gvForms.MasterTableView.Columns.FindByUniqueName("ClientSelectColumn").Visible = false;

        if (ViewState["STATUS"].ToString() == string.Empty)
        {
            dt = PhoenixDocumentManagementForm.DashboardFormCheckListDue(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
        else
        {
            dt = PhoenixDocumentManagementForm.DashboardFormCheckListExportStatusList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, ViewState["STATUS"].ToString());
        }
        gvForms.DataSource = dt;
    }
    protected void CheckAll(object sender, EventArgs e)
    {
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
    }
    protected void gvForms_ItemCommand(object sender, GridCommandEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        //string status = drv["FLDSTATUS"].ToString();
        GridEditableItem editedItem = e.Item as GridEditableItem;
        if (e.CommandName.ToUpper() == "LOCKUNLOCK")
        {
            LinkButton lock1 = (LinkButton)e.Item.FindControl("cmdLock");
            string reportid = ((RadLabel)e.Item.FindControl("lblReportId")).Text;
            PhoenixDashboardTechnical.UpdateFormStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(reportid));
            ucStatus.Text = "Unlocked Successfully";
            //HtmlGenericControl html = new HtmlGenericControl();
            //html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-unlock\"></i></span>";
            //lock1.Controls.Add(html);
            gvForms.Rebind();
        }
    }
    protected void gvForms_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton frm = (LinkButton)e.Item.FindControl("lnkForm");
        if (frm != null)
        {
            if (ViewState["STATUS"].ToString() == string.Empty)
                frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&dwpaid=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "'); return false;");
            else
                frm.Attributes.Add("onclick", "javascript:top.openNewWindow('frm','Checklist & Forms - [" + drv["FLDFORMNAME"].ToString() + "]','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + drv["FLDFORMID"].ToString() + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "&ReportId=" + drv["FLDREPORTID"].ToString() + "'); return false;");
        }
        if (e.Item is GridDataItem)
        {
            string staus = drv["FLDSTATUS"].ToString();
            LinkButton lock1 = (LinkButton)e.Item.FindControl("cmdLock");
            if (staus == "InUse on PDA")
            //if (lock1 != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lock1.CommandName)) lock1.Visible = false;
                //  lock1.Visible = true;
                //HtmlGenericControl html = new HtmlGenericControl();
                //html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-unlock\"></i></span>";
                //lock1.Controls.Add(html);
            }
            else
                lock1.Visible = false;
        }
    }
}