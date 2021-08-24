using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web;
using Telerik.Web.UI;

public partial class CrewOffshoreDocumentChecklist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                SessionUtil.PageAccessRights(this.ViewState);
                //ucConfirm.Visible = false;


                ViewState["signonoffid"] = "";
                ViewState["checklistdone"] = "";

                if (Request.QueryString["signonoffid"] != null)
                    ViewState["signonoffid"] = Request.QueryString["signonoffid"].ToString();


            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
            toolbar.AddButton("Show PDF", "SHOWPDF", ToolBarDirection.Right);
            ChecklistMenu.AccessRights = this.ViewState;
            ChecklistMenu.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreDocumentChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentChecklist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewShowExcel.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ChecklistMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CONFIRM"))
        {
            try
            {
                if (!IsValidChecklist())
                {
                    ucError.Visible = true;
                    return;
                }

                RadWindowManager1.RadConfirm("Are you sure you want to confirm.?", "confirm", 320, 150, null, "Confirm");
                //ucConfirm.Text = "Are you sure you want to confirm.?";
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
        else if (CommandName.ToUpper().Equals("SHOWPDF"))
        {
            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=11&reportcode=VESSELDOCUMENTCHECKLIST&showmenu=0&showword=0&showexcel=0&signonoffid=" + ViewState["signonoffid"].ToString());
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewOffshoreDocumentChecklist.ConfirmDocumentChecklist(int.Parse(ViewState["signonoffid"].ToString()));
            BindData();
            ucStatus.Text = "Document checklist is verified.";

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
        ViewState["checklistdone"] = "1";

        foreach (GridDataItem gr in gvDocumentChecklist.Items)
        {
            CheckBox chkOriginalReceivedYN = (CheckBox)gr.FindControl("chkOriginalReceivedYN");
            CheckBox chkPhotocopyReceivedYN = (CheckBox)gr.FindControl("chkPhotocopyReceivedYN");

            if ((chkOriginalReceivedYN != null && !chkOriginalReceivedYN.Checked) && (chkPhotocopyReceivedYN != null && !chkPhotocopyReceivedYN.Checked))
                ViewState["checklistdone"] = "0";
        }
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
                gvDocumentChecklist.Rebind();
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
            string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDAVAILABLEDOCUMENTNAME", "FLDORIGINALRECEIVEDYNNAME", "FLDPHOTOCOPYRECEIVEDYNNAME" };
            string[] alCaptions = { "Document Category", "Requirde Document", "Available Document", "Original Received Y/N", "Photocopy Received Y/N" };
            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreDocumentChecklist.SearchDocumentChecklist(General.GetNullableInteger(ViewState["signonoffid"].ToString()));
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
            string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDAVAILABLEDOCUMENTNAME", "FLDORIGINALRECEIVEDYNNAME", "FLDPHOTOCOPYRECEIVEDYNNAME" };
            string[] alCaptions = { "Document Category", "Requirde Document", "Available Document", "Original Received Y/N", "Photocopy Received Y/N" };

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreDocumentChecklist.SearchDocumentChecklist(General.GetNullableInteger(ViewState["signonoffid"].ToString()));

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

    protected void gvDocumentChecklist_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void Update_Checklist(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            CheckBox chkOriginalReceivedYN = (CheckBox)gvRow.FindControl("chkOriginalReceivedYN");
            CheckBox chkPhotocopyReceivedYN = (CheckBox)gvRow.FindControl("chkPhotocopyReceivedYN");
            string SignonoffId = ((RadLabel)gvRow.FindControl("lblSignonoffId")).Text;
            string DocType = ((RadLabel)gvRow.FindControl("lblDocType")).Text;
            string DocId = ((RadLabel)gvRow.FindControl("lblDocId")).Text;

            PhoenixCrewOffshoreDocumentChecklist.InsertDocumentChecklist(int.Parse(SignonoffId), General.GetNullableInteger(DocType), General.GetNullableInteger(DocId),
                General.GetNullableInteger(chkOriginalReceivedYN.Checked ? "1" : "0"), General.GetNullableInteger(chkPhotocopyReceivedYN.Checked ? "1" : "0"));

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindData();
        }
    }

    protected void gvDocumentChecklist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvDocumentChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            CheckBox chkPhotocopyReceivedYN = (CheckBox)e.Item.FindControl("chkPhotocopyReceivedYN");
            CheckBox chkOriginalReceivedYN = (CheckBox)e.Item.FindControl("chkOriginalReceivedYN");
            RadLabel lblDocName = (RadLabel)e.Item.FindControl("lblDocName");
            RadLabel lblAvilableDocName = (RadLabel)e.Item.FindControl("lblAvilableDocName");
            if (chkPhotocopyReceivedYN != null)
            {
                if (drv["FLDPHOTOCOPYACCEPTABLEYN"].ToString().Equals("1"))
                    chkPhotocopyReceivedYN.Enabled = true;
                else
                    chkPhotocopyReceivedYN.Enabled = false;
            }

            if (drv["FLDCONFIRMEDCHECKLISTYN"].ToString().Equals("1"))
            {
                if (chkOriginalReceivedYN != null) chkOriginalReceivedYN.Enabled = false;
                if (chkPhotocopyReceivedYN != null) chkPhotocopyReceivedYN.Enabled = false;
            }
            if (lblAvilableDocName != null && lblDocName != null)
            {
                if (lblAvilableDocName.Text.ToString().Equals(null) || lblAvilableDocName.Text.ToString() == "")
                {
                    lblDocName.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }

    protected void gvDocumentChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
}
