using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web;

public partial class CrewOffshoreAdditionalDocuments : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);               

                ViewState["empid"] = "";                

                if (Request.QueryString["empid"] != null)
                    ViewState["empid"] = Request.QueryString["empid"].ToString();                

                BindData();
            }
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
            string[] alColumns = { "FLDROWNO", "FLDTYPE", "FLDNAME"};
            string[] alCaptions = { "S.No", "Type","Name"};

            DataTable dt = new DataTable();
            dt = PhoenixCrewOffshoreCrewChange.CrewAdditionalDocumentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(ViewState["empid"].ToString()));

            gvAdditionalDoc.DataSource = dt;

           

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


    protected void gvAdditionalDoc_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
