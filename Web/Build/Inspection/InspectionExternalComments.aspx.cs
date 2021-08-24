using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionExternalComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["INTERNALINSPECTORID"] = "";
            ViewState["EXTERNALINSPECTORID"] = "";

            //PhoenixToolbar toolbar = new PhoenixToolbar();            
            //toolbar.AddButton("Schedule", "SCHEDULE");
            //toolbar.AddButton("Re-Schedule Reason", "EXTENSIONREASON");

            //MenuNCGeneral.MenuList = toolbar.Show();
            //MenuNCGeneral.SelectedMenuIndex = 1;

            if (Request.QueryString["type"].ToString().ToUpper().Equals("INTERNAL"))
                ViewState["INTERNALINSPECTORID"] = Request.QueryString["insectorid"].ToString();
            
            if (Request.QueryString["type"].ToString().ToUpper().Equals("EXTERNAL"))
                ViewState["EXTERNALINSPECTORID"] = Request.QueryString["insectorid"].ToString();

            if (Request.QueryString["type"] != null)
                ViewState["type"] = Request.QueryString["type"].ToString();
            else
                ViewState["type"] = null;

            if (Request.QueryString["inspectorname"] != null)
            {
                ViewState["inspectorname"] = Request.QueryString["inspectorname"].ToString();
            }

            if (Request.QueryString["isaudit"] != null)
            {
                ViewState["audit"] = Request.QueryString["isaudit"].ToString();
            }
            else
            {
                ViewState["audit"] = 0;
            }

            if (ViewState["inspectorname"] != null)
                txtNameOfInspector.Text = ViewState["inspectorname"].ToString();

                
            BindData();
        }
    }

    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRecordAndResponse.ExternalInspectorCommentsSearch(General.GetNullableGuid(ViewState["EXTERNALINSPECTORID"].ToString())
            , General.GetNullableGuid(Filter.CurrentSelectedInspectionSchedule) == null ? General.GetNullableGuid(Filter.CurrentAuditScheduleId) : General.GetNullableGuid(Filter.CurrentSelectedInspectionSchedule) 
                                                                                ,General.GetNullableInteger(ViewState["INTERNALINSPECTORID"].ToString())
                                                                                , General.GetNullableInteger(ViewState["audit"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["inspectorname"] == null)
                txtNameOfInspector.Text = ds.Tables[0].Rows[0]["FLDINSPECTORNAME"].ToString();

            gvExtensionReason.DataSource = ds.Tables[0];
            gvExtensionReason.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvExtensionReason);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        //SetPageNavigator();
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

}
