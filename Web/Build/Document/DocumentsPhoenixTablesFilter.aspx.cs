using System;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Document;

public partial class DocumentsPhoenixTablesFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            //if (dt.Rows.Count > 0)
            //{
            //    DynamicControlsHolder.Controls.Add(new LiteralControl("<table border='0' cellpadding='2' cellspacing='2' width='100%'>"));
            //    foreach (DataRow row in dt.Rows) // Loop over the rows.
            //    {
            //        string lblName = "lbl" + row["COLUMN_NAME"].ToString();
            //        string txtName = "txt" + row["COLUMN_NAME"].ToString();

            //        DynamicControlsHolder.Controls.Add(new LiteralControl("<tr>"));

            //        DynamicControlsHolder.Controls.Add(new LiteralControl("<td width='25%' border='0'>"));
            //        Label lbl = new Label();
            //        lbl.ID = lblName;
            //        lbl.Text = row["COLUMN_NAME"].ToString();
            //        DynamicControlsHolder.Controls.Add(lbl);
            //        DynamicControlsHolder.Controls.Add(new LiteralControl("</td>"));

            //        DynamicControlsHolder.Controls.Add(new LiteralControl("<td>"));
            //        TextBox txt = new TextBox();
            //        txt.ID = txtName;
            //        txt.CssClass = "input";
            //        DynamicControlsHolder.Controls.Add(txt);
            //        DynamicControlsHolder.Controls.Add(new LiteralControl("</td>"));

            //        DynamicControlsHolder.Controls.Add(new LiteralControl("</tr>"));


            //    }
            //    DynamicControlsHolder.Controls.Add(new LiteralControl("</TABLE>"));
            //}
            if (Request.QueryString["tablenumber"] != null && Request.QueryString["tablename"] != null)
            {
                ViewState["tablenumber"] = Request.QueryString["tablenumber"].ToString();
                ViewState["tablename"] = Request.QueryString["tablename"].ToString();
            }

        }
        if (Request.QueryString["tablename"].ToString() != "Dummy")
        {
            BindData();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            StringBuilder strQuery = new StringBuilder();

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            //criteria.Add("txtRefNo", txtRefNo.Text);
            DataTable dt = PhoenixDocumentsTables.TableColumnNameSearch(ViewState["tablename"].ToString());

            if (dt.Rows.Count > 0)
            {
                //int strFlag = 0;

                foreach (GridViewRow gvr in gvTableColumnVarcharList.Rows)
                {
                    Label lblFieldName = (Label)gvr.FindControl("lblFieldName");
                    TextBox txtValue = (TextBox)gvr.FindControl("txtValue");
                    RadioButtonList rOpertor = (RadioButtonList)gvr.FindControl("rOpertor");
                    
                    if (gvTableColumnVarcharList.Rows.Count > 0)
                    {
                        //strFlag = strFlag + 1;

                        if (txtValue.Text != "" && txtValue.Text.ToUpper() != "NULL")
                        {
                            if (rOpertor.Text.ToUpper() == "LIKE")
                                strQuery.Append(lblFieldName.Text + " " + rOpertor.Text + " '%" + txtValue.Text + "%'");
                            else
                                strQuery.Append(lblFieldName.Text + " " + rOpertor.Text + " '" + txtValue.Text + "'");

                            strQuery.Append(" AND ");
                        }
                    }
                }

                if (strQuery.Length > 0)
                {
                    strQuery.Remove(strQuery.Length - 5, 5);
                }

                
            }

            criteria.Add("txtQuery", strQuery.ToString());
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);

            Filter.CurrentPhoenixTablesSelection = criteria;
        }
        Response.Redirect("DocumentsPhoenixTables.aspx?tablenumber=" + ViewState["tablenumber"].ToString() + "&tablename=" + ViewState["tablename"].ToString() + "&showflag=1", false);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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

    protected void gvTableColumnVarcharList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //DataRowView drv = (DataRowView)e.Row.DataItem;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
        //    if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        //}
    }
    protected void gvTableColumnVarcharList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            //_gridView.EditIndex = -1;
            GridViewRow gr = _gridView.Rows[nCurrentRow];
            ((TextBox)gr.FindControl("txtValue")).Text = "";
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            DataTable dt = PhoenixDocumentsTables.TableColumnNameSearch(ViewState["tablename"].ToString());

            if (dt.Rows.Count > 0)
            {
                gvTableColumnVarcharList.DataSource = dt;
                gvTableColumnVarcharList.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvTableColumnVarcharList);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
