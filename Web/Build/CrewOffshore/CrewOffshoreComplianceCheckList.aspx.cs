using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewOffshoreComplianceCheckList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreComplianceCheckList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvComplianceList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["trainingmatrixid"] = "";
                ViewState["employeeid"] = "";
                ViewState["stage"] = "";
                ViewState["crewplanid"] = "";
                ViewState["compliance"] = "";
                ViewState["vesselid"] = "";

                if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
                    ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();

                if (Request.QueryString["employeeid"] != null && Request.QueryString["employeeid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();

                if (Request.QueryString["stage"] != null && Request.QueryString["stage"].ToString() != "")
                    ViewState["stage"] = Request.QueryString["stage"].ToString();

                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
                if (Request.QueryString["compliance"] != null && Request.QueryString["compliance"].ToString() != "")
                    ViewState["compliance"] = Request.QueryString["compliance"].ToString();
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
            }

           // BindData();
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
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDDOCTYPE", "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDDUEOVERDUENAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY" };
        string[] alCaptions = { "Document Type", "Document Category", "Document Name", "Status", "Document Number", "Issue Date", "Expiry Date" };

        DataTable dt = new DataTable();
        if(ViewState["compliance"].ToString() != "")
            dt = PhoenixCrewOffshoreComplianceCheck.OffshoreComplianceList(
                                                        General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())                                                   
                                                        , int.Parse(ViewState["employeeid"].ToString())
                                                        , int.Parse(ViewState["stage"].ToString())
                                                        , General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                        , General.GetNullableGuid(ViewState["crewplanid"].ToString()));
        else
            dt = PhoenixCrewOffshoreComplianceCheck.OffshoreStageComplianceList(
                                                            General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                                                            , General.GetNullableGuid(ViewState["crewplanid"].ToString())
                                                            , int.Parse(ViewState["employeeid"].ToString())
                                                            , int.Parse(ViewState["stage"].ToString()));

        General.ShowExcel("Compliance Check", dt, alColumns, alCaptions, null, null);
    }

    public void BindData()
    {
        string[] alColumns = { "FLDDOCTYPE", "FLDCATEGORYNAME", "FLDDOCUMENTNAME", "FLDDUEOVERDUENAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY" };
        string[] alCaptions = { "Document Type", "Document Category", "Document Name", "Status", "Document Number", "Issue Date", "Expiry Date" };

        try
        {
            DataTable dt = new DataTable();
            if(ViewState["compliance"].ToString() != "")
                dt = PhoenixCrewOffshoreComplianceCheck.OffshoreComplianceList(General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())                                                            
                                                            , int.Parse(ViewState["employeeid"].ToString())
                                                            , int.Parse(ViewState["stage"].ToString())
                                                            , General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                            , General.GetNullableGuid(ViewState["crewplanid"].ToString()));
            else
                dt = PhoenixCrewOffshoreComplianceCheck.OffshoreStageComplianceList(
                                                            General.GetNullableInteger(ViewState["trainingmatrixid"].ToString())
                                                            , General.GetNullableGuid(ViewState["crewplanid"].ToString())
                                                            , int.Parse(ViewState["employeeid"].ToString())
                                                            , int.Parse(ViewState["stage"].ToString()));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvComplianceList", "Compliance Check", alCaptions, alColumns, ds);
            gvComplianceList.DataSource = dt;
           
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvComplianceList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvComplianceList_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            RadLabel lblDueOverdue = (RadLabel)e.Item.FindControl("lblDueOverdue");
            System.Web.UI.WebControls.Image imgFlag = e.Item.FindControl("imgFlag") as System.Web.UI.WebControls.Image;

            if (lblDueOverdue != null)
            {
                if (lblDueOverdue.Text.Equals("1"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/red-symbol.png";
                }
                else if (lblDueOverdue.Text.Equals("2"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/yellow-symbol.png";
                }
                else if (lblDueOverdue.Text.Equals("3"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/green-symbol.png";
                }
                else
                {
                    imgFlag.Visible = false;
                }
            }
        }
    }
}
