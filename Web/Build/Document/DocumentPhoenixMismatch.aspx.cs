using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Document;
using SouthNests.Phoenix.Framework;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;

public partial class DocumentPhoenixMismatch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Export to Excel", "icon_xls.png", "VExcel");
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Correct Data Not in Vessel", "select.png", "Vessel");           
            MenuNotinVessel.AccessRights = this.ViewState;
            MenuNotinVessel.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Export to Excel", "icon_xls.png", "OExcel");
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Correct Data Not in Office", "select.png", "Office");           
            MenuNotinOffice.AccessRights = this.ViewState;
            MenuNotinOffice.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Export to Excel", "icon_xls.png", "MExcel");
            toolbar.AddImageButton("../Document/DocumentPhoenixMismatch.aspx", "Correct Data Mismatch", "select.png", "Mismatch");          
            MenuMismatch.AccessRights = this.ViewState;
            MenuMismatch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
                MenuOfficeFilterMain.MenuList = toolbar.Show();
                tdUpdateHeader.Visible = false;
                tdUpdater.Visible = false;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidTable(General.GetNullableString(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, ddlMismatchColumn.SelectedValue, "1"))
            {
                ucError.ErrorMessage = "Please give the value to search";
                ucError.Visible = true;
                return;
            }
            BindData();
            tdUpdateHeader.Visible = true;
            tdUpdater.Visible = true;
        }
    }
    protected void MenuMismatch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VESSEL"))
            {
                if (!IsValidTable(General.GetNullableString(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, "1", "1"))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["FLAG"] = 0;
                RadWindowManager1.RadConfirm("Are you sure want to Send the Office Data to Vessel.?", "Confirm", 320, 150, null, "Confirm");
                return;
                //ucConfirm.Text = "Are you sure want to Send the Office Data to Vessel.";
                //ucConfirm.Visible = true;
            }
            else if (CommandName.ToUpper().Equals("OFFICE"))
            {
                if (!IsValidTable(General.GetNullableString(ddlVessel.SelectedVessel), General.GetNullableString(ddlTables.SelectedTableName), "1", "1"))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["FLAG"] = 1;
                RadWindowManager1.RadConfirm("Are you sure want to Import the Vessel Data in Office.?", "Confirm", 320, 150, null, "Confirm");
                return;
                //ucConfirm.Text = "Are you sure want to Import the Vessel Data in Office.";
                //ucConfirm.Visible = true;
            }
            else if (CommandName.ToUpper().Equals("MISMATCH"))
            {
                string updatelist = string.Empty;
                foreach (ListItem li in cblUpdateColumn.Items)
                {
                    if (li.Selected) updatelist += li.Text + ",";
                }
                updatelist = updatelist.TrimEnd(',');
                if (!IsValidTable(General.GetNullableString(ddlVessel.SelectedVessel), General.GetNullableString(ddlTables.SelectedTableName)
                    , ddlMismatchColumn.SelectedValue, updatelist))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["FLAG"] = 2;
                RadWindowManager1.RadConfirm("Are you sure want to Correct Data mismatch between office and vessel.?", "Confirm", 320, 150, null, "Confirm");
                return;
                //ucConfirm.Text = "Are you sure want to Correct Data mismatch between office and vessel.";
                //ucConfirm.Visible = true;
            }
            else if (CommandName.ToUpper().Equals("VEXCEL"))
            {              
                ExportGridView(gvNotInVessel);               
            }
            else if (CommandName.ToUpper().Equals("OEXCEL"))
            {
                ExportGridView(gvNotInOffice);
            }
            else if (CommandName.ToUpper().Equals("MEXCEL"))
            {
                ExportGridView(gvMismatch);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ExportGridView(RadGrid gv)
    {
        BindData();
        PrepareGridViewForExport(gv);
        Response.ClearContent();
        Response.ContentType = "application/ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=DataMismatch.xls");
        Response.Charset = "";
        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
        gv.RenderControl(htmlwriter);
        Response.Write(stringwriter.ToString());
        Response.End();
    }
    private void BindData()
    {
        try
        {
            if (General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue
                && !string.IsNullOrEmpty(ddlTables.SelectedTableName) && !string.IsNullOrEmpty(ddlMismatchColumn.SelectedValue))
            {
                string selectlist = string.Empty;
                foreach (ListItem li in cblSelectCol.Items)
                {
                    if (li.Selected) selectlist += li.Text + ",";
                }
                selectlist = selectlist.TrimEnd(',');

                DataSet ds = PhoenixDocumentMismatch.ListMismatchData(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName
                                                                        , ddlMismatchColumn.SelectedValue, selectlist
                                                                        , General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
                if (ds.Tables.Count > 2)
                {
                    lblOfficeCount.Text = ds.Tables[0].Rows[0][0].ToString();
                    lblLastImportDate.Text = ds.Tables[0].Rows[0][1].ToString();
                    lblVesselCount.Text = ds.Tables[1].Rows[0][0].ToString();
                    if (ds.Tables[3].Rows.Count > 0)
                        lblDataNotinVesselCount.Text = ds.Tables[3].Rows[0][0].ToString();
                    if (ds.Tables[5].Rows.Count > 0)
                        lblDataNotinOfficeCount.Text = ds.Tables[5].Rows[0][0].ToString();
                    if (ds.Tables[7].Rows.Count > 0)
                        lblMismatchCount.Text = ds.Tables[7].Rows[0][0].ToString();

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        divNotinVessel.Visible = true;
                        gvNotInVessel.DataSource = ds.Tables[2];
                        gvNotInVessel.DataBind();
                    }
                    else
                    {
                        //ShowNoRecordsFound(ds.Tables[2], gvNotInVessel);
                        divNotinVessel.Visible = false;
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        divNotinOffice.Visible = true;
                        gvNotInOffice.DataSource = ds.Tables[4];
                        gvNotInOffice.DataBind();
                    }
                    else
                    {
                        //ShowNoRecordsFound(ds.Tables[4], gvNotInOffice);
                        divNotinOffice.Visible = false;
                    }
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        divMismatch.Visible = true;
                        gvMismatch.DataSource = ds.Tables[6];
                        gvMismatch.DataBind();
                    }
                    else
                    {
                        //ShowNoRecordsFound(ds.Tables[6], gvMismatch);
                        divMismatch.Visible = false;
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
    private bool IsValidTable(string VesselId, string StrTableName, string MismatchColumnName, string UpdateColumnName)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (VesselId == null)
        {
            ucError.ErrorMessage = "Please Select a Vessel a Compare Office and Vessel Data";
        }
        if (StrTableName == null)
        {
            ucError.ErrorMessage = "Please Select a Table";
        }
        if (string.IsNullOrEmpty(MismatchColumnName))
        {
            ucError.ErrorMessage = "Please Select a MismatchColumn to Compare the Office and Vessel data";
        }
        if (string.IsNullOrEmpty(UpdateColumnName))
        {
            ucError.ErrorMessage = "Please Select list of Update Column that needs to be updated";
        }
        if (General.GetNullableDateTime(txtDate.Text).HasValue
            && DateTime.TryParse(txtCompareFrom.Text, out resultDate) && DateTime.Compare(resultDate, General.GetNullableDateTime(txtDate.Text).Value) > 0)
        {
            ucError.ErrorMessage = "Comparison From should be earlier than Comparison Date";
        }
        return (!ucError.IsError);
    }
    protected void ddlTables_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlVesselTables ddl = (UserControlVesselTables)sender;
        if (!string.IsNullOrEmpty(ddl.SelectedTableName))
        {
            DataTable dt = PhoenixDocumentsTables.TableColumnNameSearch(ddl.SelectedTableName);
            ddlMismatchColumn.DataSource = dt;
            ddlMismatchColumn.DataBind();
            ddlMismatchColumn.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            cblUpdateColumn.DataSource = dt;
            cblUpdateColumn.DataBind();
            cblSelectCol.DataSource = dt;
            cblSelectCol.DataBind();
        }
    }
    //protected void btnApprove_Click(object sender, EventArgs e)
    //{
    //    UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;        
    //    if (ucCM.confirmboxvalue == 1)
    //    {
    //        if (ViewState["FLAG"].ToString() == "0")
    //        {
    //            PhoenixDocumentMismatch.UpdateMismatchDataNotinVessel(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
    //        }
    //        if (ViewState["FLAG"].ToString() == "1")
    //        {
    //            PhoenixDocumentMismatch.UpdateMismatchDataNotinOffice(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
    //        }
    //        if (ViewState["FLAG"].ToString() == "2")
    //        {
    //            string updatelist = string.Empty;
    //            foreach (ListItem li in cblUpdateColumn.Items)
    //            {
    //                if (li.Selected) updatelist += li.Text + ",";
    //            }
    //            updatelist = updatelist.TrimEnd(',');
    //            PhoenixDocumentMismatch.UpdateMismatchData(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, ddlMismatchColumn.SelectedValue, updatelist, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
    //        }
    //        BindData();
    //    }
    //}
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {            
            if (FileUpload.PostedFile.ContentLength > 0)
            {                
                string path = Server.MapPath("~/Attachments/Query/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filename = FileUpload.PostedFile.FileName;
                if (filename.LastIndexOf("\\") >= 0)
                {
                    filename = filename.Substring(filename.LastIndexOf("\\") + 1);
                }
                FileUpload.PostedFile.SaveAs(path + filename);
                PhoenixDocumentMismatch.RunImportQuery();
            }
        }      
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    /// <summary>
    /// This event is used to verify the form control is rendered
    /// It is used to remove the error occuring while exporting to export
    /// The Error is : Control 'XXX' of type 'GridView' must be placed inside a form tag with runat=server.
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    /// <summary>
    /// Replace any container controls with literals
    /// like Hyperlink, ImageButton, LinkButton, DropDown, ListBox to literals
    /// </summary>
    /// <param name="gridView">GridView</param>
    private void PrepareGridViewForExport(Control gridView)
    {
        for (int i = 0; i < gridView.Controls.Count; i++)
        {
            //Get the control
            Control currentControl = gridView.Controls[i];
            if (currentControl is LinkButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
            }
            else if (currentControl is ImageButton)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
            }
            else if (currentControl is HyperLink)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
            }
            else if (currentControl is DropDownList)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
            }
            else if (currentControl is CheckBox)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
            }
            if (currentControl.HasControls())
            {
                // if there is any child controls, call this method to prepare for export
                PrepareGridViewForExport(currentControl);
            }
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        if (ViewState["FLAG"].ToString() == "0")
        {
            PhoenixDocumentMismatch.UpdateMismatchDataNotinVessel(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
        }
        if (ViewState["FLAG"].ToString() == "1")
        {
            PhoenixDocumentMismatch.UpdateMismatchDataNotinOffice(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
        }
        if (ViewState["FLAG"].ToString() == "2")
        {
            string updatelist = string.Empty;
            foreach (ListItem li in cblUpdateColumn.Items)
            {
                if (li.Selected) updatelist += li.Text + ",";
            }
            updatelist = updatelist.TrimEnd(',');
            PhoenixDocumentMismatch.UpdateMismatchData(int.Parse(ddlVessel.SelectedVessel), ddlTables.SelectedTableName, ddlMismatchColumn.SelectedValue, updatelist, General.GetNullableDateTime(txtDate.Text), General.GetNullableDateTime(txtCompareFrom.Text));
        }
        BindData();
    }
}
