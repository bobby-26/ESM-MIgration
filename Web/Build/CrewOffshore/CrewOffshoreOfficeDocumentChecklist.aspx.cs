using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewOffshoreOfficeDocumentChecklist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentChecklist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreOfficeDocumentChecklist.aspx", "Refresh", "<i class=\"fas fa-sync\"></i>", "REFRESH");
           
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                ucConfirm.Visible = false;
               
                //toolbar.AddButton("Confirm", "CONFIRM");
                //toolbar.AddButton("Show PDF", "SHOWPDF");
                //ChecklistMenu.AccessRights = this.ViewState;
                //ChecklistMenu.MenuList = toolbar.Show();

               

                ViewState["signonoffid"] = "";
                ViewState["checklistdone"] = "";

                if (Request.QueryString["signonoffid"] != null)
                    ViewState["signonoffid"] = Request.QueryString["signonoffid"].ToString();

                if (Request.QueryString["Crewplanid"] != null)
                    ViewState["Crewplanid"] = Request.QueryString["Crewplanid"].ToString();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ChecklistMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
       
        //    if (dce.CommandName.ToUpper().Equals("CONFIRM"))
        //    {
        //        if (!IsValidChecklist())
        //        {
        //            ucError.Visible = true;
        //            return;
        //        }
        //        ucConfirm.Visible = true;
        //        ucConfirm.Text = "Are you sure you want to confirm.?";
        //        return;
        //    }
        //    else if (dce.CommandName.ToUpper().Equals("SHOWPDF"))
        //    {
        //        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=11&reportcode=VESSELDOCUMENTCHECKLIST&showmenu=0&showword=0&showexcel=0&signonoffid=" + ViewState["signonoffid"].ToString());
        //    }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
            //    PhoenixCrewOffshoreDocumentChecklist.ConfirmDocumentChecklist(int.Parse(ViewState["signonoffid"].ToString()));
            //    BindData();
            //    ucStatus.Text = "Document checklist is verified.";
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidChecklist()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        BindData();
        CheckDocumentChecklist();

        if (General.GetNullableInteger(ViewState["checklistdone"].ToString()) == 0)
        {
            ucError.ErrorMessage = "All documents must be checked and collected before confirmation.";
        }

        return (!ucError.IsError);
    }

    protected void CheckDocumentChecklist()
    {
        //ViewState["checklistdone"] = "1";

        //foreach (GridViewRow gr in gvDocumentChecklist.Rows)
        //{
        //    //CheckBox chkOriginalReceivedYN = (CheckBox)gr.FindControl("chkOriginalReceivedYN");
        //    //CheckBox chkPhotocopyReceivedYN = (CheckBox)gr.FindControl("chkPhotocopyReceivedYN");

        //    //if ((chkOriginalReceivedYN != null && !chkOriginalReceivedYN.Checked) && (chkPhotocopyReceivedYN != null && !chkPhotocopyReceivedYN.Checked))
        //    //    ViewState["checklistdone"] = "0";
        //}
    }

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixCrewOffshoreCheckList.InsertDocumentCheckList(General.GetNullableGuid(ViewState["Crewplanid"].ToString()));
                BindData();
            }
            if (CommandName.ToUpper() == "SAVE")
            {
                string doclistid = "";
                string remarks = "";
                string verified = "";
                foreach (GridDataItem item in gvDocumentChecklist.Items)
                {
                    doclistid += ((RadLabel)item.FindControl("lbldoclistid")).Text + ",";
                    remarks += (((RadTextBox)item.FindControl("txtremarkedit")).Text.Trim()) + ",";
                    verified += (((CheckBox)item.FindControl("chkverify")).Checked ? 1 : 0) + ",";

                }
                PhoenixCrewOffshoreCheckList.UpdateDocumentCheckRemark(doclistid, remarks, verified);
                ucStatus.Text = "Document checklist is verified.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDAVAILABLEDOCUMENTNAME", "FLDCERTIFICATENO", "FLDDATEOFEXPIRY" };
            string[] alCaptions = { "Document Category", "Requirde Document", "Available Document","Certificate No.","Expiry Date" };
            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreDocumentChecklist.SearchDocumentChecklist(General.GetNullableInteger(ViewState["signonoffid"].ToString()),
                General.GetNullableGuid(ViewState["Crewplanid"].ToString()));
            General.ShowExcel("Document Checklist", dt, alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDAVAILABLEDOCUMENTNAME" };
            string[] alCaptions = { "Document Category", "Requirde Document", "Available Document" };

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreDocumentChecklist.SearchDocumentChecklist(General.GetNullableInteger(ViewState["signonoffid"].ToString()),
                General.GetNullableGuid(ViewState["Crewplanid"].ToString()));


            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvDocumentChecklist", "Document Checklist", alCaptions, alColumns, ds);
            gvDocumentChecklist.DataSource = dt;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
 
    //protected void Update_Checklist(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox cb = (CheckBox)sender;
    //        GridViewRow gvRow = (GridViewRow)cb.Parent.Parent;

    //        CheckBox chkOriginalReceivedYN = (CheckBox)gvRow.FindControl("chkOriginalReceivedYN");
    //        CheckBox chkPhotocopyReceivedYN = (CheckBox)gvRow.FindControl("chkPhotocopyReceivedYN");
    //        string SignonoffId = ((Label)gvRow.FindControl("lblSignonoffId")).Text;
    //        string DocType = ((Label)gvRow.FindControl("lblDocType")).Text;
    //        string DocId = ((Label)gvRow.FindControl("lblDocId")).Text;

    //        PhoenixCrewOffshoreDocumentChecklist.InsertDocumentChecklist(int.Parse(SignonoffId), General.GetNullableInteger(DocType), General.GetNullableInteger(DocId),
    //            General.GetNullableInteger(chkOriginalReceivedYN.Checked ? "1" : "0"), General.GetNullableInteger(chkPhotocopyReceivedYN.Checked ? "1" : "0"));

    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        BindData();
    //    }
    //}

    protected void gvDocumentChecklist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
       BindData();
       
    }

    protected void gvDocumentChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvDocumentChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
           
            RadLabel lblDocName = (RadLabel)e.Item.FindControl("lblDocName");
            RadLabel lblAvilableDocName = (RadLabel)e.Item.FindControl("lblAvilableDocName");
          
            if (lblAvilableDocName != null && lblDocName != null)
            {
                if (lblAvilableDocName.Text.ToString().Equals(null) || lblAvilableDocName.Text.ToString() == "")
                {
                    lblDocName.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
