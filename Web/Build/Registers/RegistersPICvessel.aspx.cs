using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class RegistersPICvessel : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddImageButton("../Registers/RegistersPICvessel.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar1.AddImageButton("../Registers/RegistersPICvessel.aspx", "Find", "search.png", "FIND");
        toolbar1.AddImageButton("../Registers/RegistersPICvessel.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
       
        MenuPCIAdmin.AccessRights = this.ViewState;
        MenuPCIAdmin.MenuList = toolbar1.Show();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        MenuPicVessel.AccessRights = this.ViewState;
        MenuPicVessel.MenuList = toolbar.Show();
        string temp = txtFileUpload1.FileName.ToString();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvVesselAdminUser.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            Bindinvoicetype();
            Bindinvoicestatus();
           
        }

    //    BindData();

    }

    protected void ddlInvoiceTypeSelectedindexchange(object sender, EventArgs e)
    {
        Bindinvoicestatus();
        BindData();
       
    }
    public void Bindinvoicetype()
    {
        DataSet ds = PhoenixRegistersDesignationInvoiceStatus.PICinvoicetyepeList(1);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            ddlInvoiceType.DataSource = dt;
            ddlInvoiceType.DataTextField = "FLDINVOICETYPENAME";
            ddlInvoiceType.DataValueField = "FLDINVOICETYPE";
            ddlInvoiceType.DataBind();
            ddlInvoiceType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        }
    }

    public void Bindinvoicestatus()
    {
        DataSet ds = PhoenixRegistersDesignationInvoiceStatus.PICinvoicestatusList(1, General.GetNullableInteger(ddlInvoiceType.SelectedValue));
        DataTable dt = ds.Tables[0];

        ddlInvoiceStatus.DataSource = dt;
        ddlInvoiceStatus.DataTextField = "FLDINVOICESTATUSNAME";
        ddlInvoiceStatus.DataValueField = "FLDINVOICESTATUS";
        ddlInvoiceStatus.DataBind();
        ddlInvoiceStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }

    protected void MenuPCIAdmin_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {

            Clearfilter();
            BindData();
          
        }

    }
    public void Clearfilter()
    {   
        ddlVessel.SelectedVessel = "Dummy";
        ddlInvoiceType.SelectedValue = "Dummy";
        ddlInvoiceStatus.SelectedValue = "Dummy";
       
    }

    protected void MenuPicVessel_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                if (!IsValidInvoiceStatusType(ddlInvoiceType.SelectedValue, ddlInvoiceStatus.SelectedValue, ucPIC.SelectedUser, txtFileUpload1.FileName.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                int designation;
                Guid designationinvoiceid;
                DataSet ds=PhoenixRegistersDesignationInvoiceStatus.PICVesselDesignation(1,General.GetNullableInteger(ddlInvoiceType.SelectedValue.ToString()),
                    General.GetNullableInteger(ddlInvoiceStatus.SelectedValue.ToString()));
                DataTable dt=ds.Tables[0];
                if(dt.Rows.Count>0)
                {
                    DataRow dr=dt.Rows[0];
                    designation=Convert.ToInt32(dr["FLDDESIGNATIONID"].ToString());
                    designationinvoiceid = new Guid (dr["FLDDESIGNATIONINVOICEID"].ToString());
                }
                else
                {
                    ucError.ErrorMessage = "Designation not Define for Invoice type and Invoice Ststus Combination.";
                        ucError.Visible = true;
                        return;
                }
               
                string fileName = txtFileUpload1.FileName.ToString();
                string extension = Path.GetExtension(txtFileUpload1.FileName.ToString());
                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Accounts/") + fileName + extension;
                    txtFileUpload1.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        PICVesselUpload(strpath, fileName, designation, designationinvoiceid);
                        BindData();
                     
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please upload correct file with data.";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please upload .xlsx file only";
                    ucError.Visible = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

    }



    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }

    private bool IsValidInvoiceStatusType(string invoicetype, string invoicestatus, string pic, string Attachment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(invoicetype) == null)
        {
            ucError.ErrorMessage = "Invoice Type is required.";
        }
        if (General.GetNullableInteger(invoicestatus) == null)
        {
            ucError.ErrorMessage = "Invoice Status is required.";
        }
        if (General.GetNullableInteger(pic) == null)
        {
            ucError.ErrorMessage = "PIC is required.";
        }
        if (General.GetNullableString(Attachment) == null)
        {
            ucError.ErrorMessage = "Attachment is required.";
        }

        return (!ucError.IsError);
    }

    public void PICVesselUpload(string filepath, string FileName, int InvoiceDesignationID, Guid designationinvoiceid)
    {
        try
        {


            string patchid = Guid.NewGuid().ToString();
            List<string> cellvalues = new List<string>();
            var package = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        int endRow = 1;

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }

                        for (int j = workSheet.Dimension.Start.Column; j <= 1; j++)
                        {
                            if (!workSheet.Cells[1, j].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[1, j].Value));
                        }
                        if (!VerifyHeaders(cellvalues))
                        {
                            ucError.ErrorMessage = "File is of incorrect format";
                            ucError.Visible = true;
                            return;
                        }

                        cellvalues.Clear();


                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 1; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));

                            }

                            int? vesselid = General.GetNullableInteger(workSheet.Cells[i, 1].Value == null ? null : workSheet.Cells[i, 1].Value.ToString());

                            if (vesselid == null)
                            {
                                ucError.ErrorMessage = "Vessel ID in row" + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            else
                            {
                                DataSet ds = PhoenixRegistersDesignationInvoiceStatus.CheckVesselID(vesselid);
                                if (ds.Tables[0].Rows.Count <= 0)
                                {
                                    ucError.ErrorMessage = "Vessel ID in row" + i.ToString() + " is not Available in vessel List";
                                    ucError.Visible = true;
                                    return;
                                }
                            }

                            //string vesselname = General.GetNullableString(workSheet.Cells[i, 2].Value == null ? null : workSheet.Cells[i, 1].Value.ToString());

                            //if (vesselname == null)
                            //{
                            //    ucError.ErrorMessage = "Vessel Name in row " + i.ToString() + " is not in correct format";
                            //    ucError.Visible = true;
                            //    return;
                            //}

                            //string vesselshortcode = General.GetNullableString(workSheet.Cells[i, 3].Value == null ? null : workSheet.Cells[i, 1].Value.ToString());

                            //if (vesselshortcode == null)
                            //{
                            //    ucError.ErrorMessage = "Vessel Short Code in row " + i.ToString() + " is not in correct format";
                            //    ucError.Visible = true;
                            //    return;
                            //}
                            cellvalues.Clear();
                        }
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            DataSet ds = PhoenixRegistersDesignationInvoiceStatus.PICVesselMaplist(General.GetNullableInteger((Convert.ToString(workSheet.Cells[i, 1].Value))), InvoiceDesignationID, designationinvoiceid);
                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];
                                Guid VesselAdminUserMapCode =new Guid(dr["FLDVESSELADMINUSERMAPCODE"].ToString());
                                UpdateVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode, InvoiceDesignationID, Convert.ToInt32(ucPIC.SelectedUser));
                            }
                            else
                            {
                                InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger((Convert.ToString(workSheet.Cells[i, 1].Value))), InvoiceDesignationID, Convert.ToInt32(ucPIC.SelectedUser), designationinvoiceid);
                            }
                            cellvalues.Clear();
                        }

                    }
                }
            }

        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }


    private bool VerifyHeaders(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("VESSEL ID"))
            return false;
        //else if (!li[1].ToString().ToUpper().Equals("VESSEL NAME"))
        //    return false;
        //else if (!li[2].ToString().ToUpper().Equals("VESSEL SHORT CODE"))
        //    return false;
        else
            return true;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDTYPENAME", "FLDSTATUSNAME", "FLDPICUSERNAME" };
        string[] alCaptions = { "Vessel Id", "Vessel Name", "Vessel Code", "Invoice Type", "Invoice Status Name ","Designation Name", "PIC Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        int? vesselid = General.GetNullableInteger(ddlVessel.SelectedVessel.ToString());
        int? invoicestatus = General.GetNullableInteger(ddlInvoiceStatus.SelectedValue);
        int? invoicetype = General.GetNullableInteger(ddlInvoiceType.SelectedValue);

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


         ds = PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapSearch(vesselid, sortexpression, sortdirection,
                1,
                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount, 1);


        Response.AddHeader("Content-Disposition", "attachment; filename=Vessel PIC.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PIC for Vessel</h3></td>");
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
            for (int i = 4; i < alColumns.Length; i++)
            {
                Response.Write("<td>");

                Response.Write(ddlVessel.SelectedVessel.ToString() == "Dummy" ? "" : ddlVessel.SelectedVessel.ToString());
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write(ddlVessel.SelectedVessel.ToString() == "Dummy" ? "" :ddlVessel.SelectedVesselName);
                Response.Write("</td>");
                Response.Write("<td>");
                string vesselcode = string.Empty;
                int? vessel = General.GetNullableInteger(ddlVessel.SelectedVessel.ToString());
                DataSet dataset = PhoenixRegistersDesignationInvoiceStatus.CheckVesselID(vessel);
                DataTable datatable = dataset.Tables[0];
                if (datatable.Rows.Count > 0)
                {
                    DataRow datarow = datatable.Rows[0];
                    vesselcode = datarow["FLDVESSELCODE"].ToString();
                }
                Response.Write(vesselcode);
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write(dr["FLDTYPENAME"]);
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write(dr["FLDSTATUSNAME"]);
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write(dr["FLDDESIGNATIONNAME"]);
                Response.Write("</td>");
                Response.Write("<td>");
                Response.Write(dr["FLDPICUSERNAME"]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void Rebind()
    {
        gvVesselAdminUser.SelectedIndexes.Clear();
        gvVesselAdminUser.EditIndexes.Clear();
        gvVesselAdminUser.DataSource = null;
        gvVesselAdminUser.Rebind();
    }
    protected void gvVesselAdminUser_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselAdminUser.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDINVOICETYPENAME", "FLDVESSELDISCOUNTPERCENTAGE" };
        string[] alCaptions = { "Vessel Name", "SupplierCode", "Supplier Name", "%Return" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        int? vesselid = General.GetNullableInteger(ddlVessel.SelectedVessel.ToString());
        int? invoicestatus = General.GetNullableInteger(ddlInvoiceStatus.SelectedValue);
        int? invoicetype = General.GetNullableInteger(ddlInvoiceType.SelectedValue);
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapSearch(vesselid,sortexpression, sortdirection,
                1,
                gvVesselAdminUser.PageSize,
                ref iRowCount,
                ref iTotalPageCount, 1);

        General.SetPrintOptions("gvVesselAdminUser", "VESSEL PIC", alCaptions, alColumns, ds);

        gvVesselAdminUser.DataSource = ds;
        gvVesselAdminUser.VirtualItemCount = iRowCount;

      

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
       


    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
      
    }
    //protected void gvVesselAdminUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}

    protected void gvVesselAdminUser_SortCommand(object sender, GridSortCommandEventArgs e)
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

    //protected void gvVesselAdminUser_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvVesselAdminUser, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    protected void gvVesselAdminUser_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;


                int UserDesignation = int.Parse(((UserControlDesignation)e.Item.FindControl("ucDesignation")).SelectedDesignation);
                int PICUserId = int.Parse(((UserControlUserName)e.Item.FindControl("ucPIC")).SelectedUser);

                //InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentVesselMasterFilter.ToString()), UserDesignation, PICUserId);
                ucStatus.Text = "Vessel Admin Uesr Information added.";
                Rebind();



            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                if (!IsValidData(((UserControlUserName)e.Item.FindControl("ucPICEdit")).SelectedUser))
                {

                    ucError.Visible = true;
                    return;
                }
                int UserDesignation = int.Parse(((RadLabel)e.Item.FindControl("lblDesignationId")).Text);
                int PICUserId = int.Parse(((UserControlUserName)e.Item.FindControl("ucPICEdit")).SelectedUser);
                Guid designationinvoiceid = new Guid(((RadLabel)e.Item.FindControl("lblDesignationInvoiceId")).Text);

                if (((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCodeEdit")).Text == "")
                {
                    InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlVessel.SelectedVessel.ToString()), UserDesignation, PICUserId, designationinvoiceid);
                }
                else
                {
                    Guid VesselAdminUserMapCode = new Guid(((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCodeEdit")).Text);
                    UpdateVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode, UserDesignation, PICUserId);
                }
                ucStatus.Text = "Vessel Admin Uesr Information updated.";
              
                Rebind();
              

            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                Guid VesselAdminUserMapCode = new Guid(((RadLabel)e.Item.FindControl("lblVesselAdminUserMapCode")).Text);
                PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode);
                Rebind();
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    //protected void gvVesselAdminUser_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {


    //            int UserDesignation = int.Parse(((UserControlDesignation)_gridView.FooterRow.FindControl("ucDesignation")).SelectedDesignation);
    //            int PICUserId = int.Parse(((UserControlUserName)_gridView.FooterRow.FindControl("ucPIC")).SelectedUser);

    //            //InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentVesselMasterFilter.ToString()), UserDesignation, PICUserId);
    //            ucStatus.Text = "Vessel Admin Uesr Information added.";
    //            BindData();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("UPDATE"))
    //        {
    //            if (!IsValidData(((UserControlUserName)_gridView.Rows[nCurrentRow].FindControl("ucPICEdit")).SelectedUser))
    //            {

    //                ucError.Visible = true;
    //                return;
    //            }
    //            int UserDesignation = int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDesignationId")).Text);
    //            int PICUserId = int.Parse(((UserControlUserName)_gridView.Rows[nCurrentRow].FindControl("ucPICEdit")).SelectedUser);
    //            Guid designationinvoiceid = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDesignationInvoiceId")).Text);

    //            if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselAdminUserMapCodeEdit")).Text == "")
    //            {
    //                InsertVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlVessel.SelectedVessel.ToString()), UserDesignation, PICUserId, designationinvoiceid);
    //            }
    //            else
    //            {
    //                Guid VesselAdminUserMapCode = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselAdminUserMapCodeEdit")).Text);
    //                UpdateVesselAdminUser(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode, UserDesignation, PICUserId);
    //            }
    //            ucStatus.Text = "Vessel Admin Uesr Information updated.";
    //            _gridView.EditIndex = -1;
    //            BindData();
    //            SetPageNavigator();
    //        }


    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            Guid VesselAdminUserMapCode = new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselAdminUserMapCode")).Text);
    //            PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselAdminUserMapCode);
    //            BindData();
    //        }

    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    //protected void gvVesselAdminUser_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvVesselAdminUser_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvVesselAdminUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //    SetPageNavigator();
    //}

   
    protected void gvVesselAdminUser_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                ImageButton add = (ImageButton)e.Item.FindControl("cmdAdd");
                if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            }

            if (e.Item is GridDataItem)
            {
                if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtSupplierIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                ImageButton ib1 = (ImageButton)e.Item.FindControl("btnSuppllierEdit");
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx')");
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
                lblvesselid.Text = ddlVessel.SelectedVessel.ToString() == "Dummy" ? "" : ddlVessel.SelectedVessel.ToString();
                if (lblvesselid.Text != "" && lblvesselid.Text != "Dummy")
                {
                    string vesselcode = string.Empty;
                    int? vesselid = General.GetNullableInteger(lblvesselid.Text.ToString());
                    DataSet ds = PhoenixRegistersDesignationInvoiceStatus.CheckVesselID(vesselid);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        vesselcode = dr["FLDVESSELCODE"].ToString();
                    }
                    RadLabel lblvesselName = (RadLabel)e.Item.FindControl("lblvesselName");
                    lblvesselName.Text = ddlVessel.SelectedVesselName;
                    RadLabel lblvesselcode = (RadLabel)e.Item.FindControl("lblvesselcode");
                    lblvesselcode.Text = vesselcode;
                }
                UserControlDesignation ucDesignationEdit = (UserControlDesignation)e.Item.FindControl("ucDesignationEdit");
                //DataRowView drv = (DataRowView)e.Row.DataItem;
                //if (ucDesignationEdit != null) ucDesignationEdit.SelectedDesignation = drv["FLDDESIGNATIONID"].ToString();
                DataRowView drv = (DataRowView)e.Item.DataItem;
                UserControlUserName ucPICEdit = (UserControlUserName)e.Item.FindControl("ucPICEdit");
                if (ucPICEdit != null) ucPICEdit.SelectedUser = drv["FLDPICUSERID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
      
        BindData();
     
    }

  
    private void InsertVesselAdminUser(int rowusercode, int? Vesselid, int DesignationId, int PICUserId, Guid designationinvoiceid)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapInsert(rowusercode, Vesselid, null, DesignationId, PICUserId, designationinvoiceid);
    }

    private void UpdateVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode, int DesignationId, int PICUserId)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapUpdate(rowusercode, VesselAdminUserMapCode, DesignationId, PICUserId);
    }

    private bool IsValidData( string userid)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (userid.Trim().Equals("") || userid.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "PIC User is required.";
        return (!ucError.IsError);
    }

    private void DeleteVesselAdminUser(int rowusercode, Guid VesselAdminUserMapCode)
    {
        PhoenixRegistersVesselOfficeAdmin.VesselAdminUserMapDelete(rowusercode, VesselAdminUserMapCode);
    }

  
    protected void lnkDownloadExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\PIC_ADMIN-VESSEL.xlsx";
            string destinationpath = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + Guid.NewGuid().ToString() + ".xlsx";

            FileInfo fiTemplate = new FileInfo(file);
            fiTemplate.CopyTo(destinationpath);

            FileInfo fidestination = new FileInfo(destinationpath);

            using (ExcelPackage pck = new ExcelPackage(fidestination))
            {
                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12 .xltm";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=PIC_ADMIN-VESSEL.xlsx");
                HttpContext.Current.Response.End();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
