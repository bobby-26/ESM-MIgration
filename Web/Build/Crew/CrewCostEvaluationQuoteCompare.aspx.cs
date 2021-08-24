using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
public partial class CrewCostEvaluationQuoteCompare : PhoenixBasePage
{
    ArrayList arrayPort = new ArrayList();
    ArrayList arrayportid = new ArrayList();

    ArrayList arrayAgent = new ArrayList();
    ArrayList arrayagentid = new ArrayList();

    ArrayList arraySectiontype = new ArrayList();
    ArrayList arraysectiontypeid = new ArrayList();

    int cnt, agentscnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Back", "BACK");
            MenuQuotationCompare.AccessRights = this.ViewState;
            MenuQuotationCompare.MenuList = toolbar.Show();
            MenuQuotationCompare.Visible = true;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["requestid"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                    ViewState["PORTAGENTS"] = Request.QueryString["agents"].ToString();

                }
                else
                {
                    ViewState["REQUESTID"] = "0";
                    ViewState["selectagents"] = "";
                }
                SetInformation();
            }
            BindData();
            BindSectionType();
            BindAirfare();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetInformation()
    {
        frmTitle.Text = "Quotation Compare(" + PhoenixCrewCostEvaluationRequest.RequestNumber + ")";
    }
    protected void MenuQuotationCompare_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Crew/CrewCostEvaluationQuoteAgent.aspx?requestid=" + ViewState["REQUESTID"].ToString());
        }
    }


    private void BindData()
    {                 
        DataSet ds = PhoenixCrewCostEvaluationQuote.QuotationCompareSection(new Guid(ViewState["REQUESTID"].ToString()), ViewState["PORTAGENTS"].ToString());

        arrayPort.Clear();
        cnt = ds.Tables[2].Rows.Count; // Port Count
        agentscnt = ds.Tables[3].Rows.Count; // Agents Count

        foreach (DataRow dr in ds.Tables[2].Rows)
        {
            arrayPort.Add(dr["FLDSEAPORTNAME"].ToString());
            arrayportid.Add(dr["FLDEVALUATIONPORTID"].ToString());

        }
        foreach (DataRow dr in ds.Tables[3].Rows)
        {
            arrayAgent.Add(dr["FLDNAME"].ToString());
            arrayagentid.Add(dr["FLDPORTAGENTID"].ToString());

        }


        if (ds.Tables[0].Rows.Count > 0)
        {
            AddCoumnsInGrid(ds.Tables[0], ds.Tables[2], ds.Tables[1]);

            gvSection.DataSource = ds;
            gvSection.DataBind();
            if (gvSection.Columns.Count > 2)
            {
                //CheckLowestAmount();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSection);
        } 
    }

    private void BindSectionType()
    {
        DataSet ds = PhoenixCrewCostEvaluationQuote.QuotationCompareSectionTypeTotal(new Guid(ViewState["REQUESTID"].ToString()));

        arrayPort.Clear();
        arrayportid.Clear();
        arrayAgent.Clear();
        arrayagentid.Clear();

        cnt = ds.Tables[2].Rows.Count; // Port Count
        agentscnt = ds.Tables[3].Rows.Count; // Agents Count

        foreach (DataRow dr in ds.Tables[2].Rows)
        {
            arrayPort.Add(dr["FLDSEAPORTNAME"].ToString());
            arrayportid.Add(dr["FLDEVALUATIONPORTID"].ToString());

        }
        foreach (DataRow dr in ds.Tables[3].Rows)
        {
            arrayAgent.Add(dr["FLDNAME"].ToString());
            arrayagentid.Add(dr["FLDPORTAGENTID"].ToString());

        }


        if (ds.Tables[0].Rows.Count > 0)
        {
            AddCoumnsInGridSectionType(ds.Tables[0], ds.Tables[2], ds.Tables[1]);

            gvSecType.DataSource = ds;
            gvSecType.DataBind();
            if (gvSecType.Columns.Count > 2)
            {                
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSecType);
        } 
    }

    private void BindAirfare()
    {
        try
        {

            arrayPort.Clear();
            arrayportid.Clear();

            DataSet ds = new DataSet();

            ds = PhoenixCrewCostEvaluationRequest.ListCrewCostEvaluationAirefareCompare(General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

            cnt = ds.Tables[2].Rows.Count;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                arrayPort.Add(dr["FLDSEAPORTNAME"].ToString());
                arrayportid.Add(dr["FLDEVALUATIONPORTID"].ToString());

            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                AddCoumnsInGridAirfare(ds.Tables[0], ds.Tables[1], ds.Tables[2]);

                gvCrewAirfare.DataSource = ds;
                gvCrewAirfare.DataBind();               
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewAirfare);
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CheckLowestAmount()
    {
        if (gvSection.Columns.Count > 2)
        {
            foreach (GridViewRow gvr in gvSection.Rows)
            {
                decimal lowamount = CheckDecimal(gvr.Cells[8].Text);
                //int k = 8;
                //for (int i = 8; i < gvSection.Columns.Count; i += 4)
                //{
                //    if (lowamount > CheckDecimal(gvr.Cells[i].Text) && (CheckDecimal(gvr.Cells[i].Text) > 0))
                //    {
                //        lowamount = CheckDecimal(gvr.Cells[i].Text);
                //        k = i;
                //    }
                //}
                //gvr.Cells[4].Text = lowamount.ToString();
                //gvr.Cells[k].ForeColor = System.Drawing.Color.Chocolate;
            }
        }

    }

    private decimal CheckDecimal(string decimalvalue)
    {
        if (General.GetNullableDecimal(decimalvalue) != null)
            return decimal.Parse(decimalvalue);
        else
            return decimal.Parse("0.00");
    }
    //  string port = arrayPort[j].ToString();
    private void AddCoumnsInGrid(DataTable datatable, DataTable agenttable, DataTable totaltable)
    {
        if (datatable.Columns.Count > 1 && gvSection.Columns.Count < 6)
        {
            int k = 0;

            for (int i = 2; i < datatable.Columns.Count - 1; i = i + 1)
            {
                BoundField amountboundfield = new BoundField();

                amountboundfield.DataField = datatable.Columns[i].ColumnName.ToString();
                string stragent = arrayAgent[k].ToString().Trim();
                amountboundfield.HeaderText = stragent;// getFirstWord(stragent);
                amountboundfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                string strcolname = totaltable.Columns[i].ColumnName.ToString();
                amountboundfield.FooterText = totaltable.Rows[0][strcolname].ToString();

                amountboundfield.DataFormatString = "{0:n2}";
                amountboundfield.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                gvSection.Columns.Add(amountboundfield);

                if (k >= agentscnt - 1)
                    k = 0;
                else
                    k = k + 1;
            }
        }
    }

    private void AddCoumnsInGridSectionType(DataTable datatable, DataTable agenttable, DataTable totaltable)
    {
        if (datatable.Columns.Count > 1 && gvSecType.Columns.Count < 6)
         {
            int k = 0;

            for (int i = 1; i < datatable.Columns.Count - 1; i = i + 1)
            {
                BoundField amountboundfield = new BoundField();

                amountboundfield.DataField = datatable.Columns[i].ColumnName.ToString();
                amountboundfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                string stragent = arrayAgent[k].ToString().Trim();
                amountboundfield.HeaderText = stragent;// getFirstWord(stragent);
                string strcolname = totaltable.Columns[i].ColumnName.ToString();
                amountboundfield.FooterText = totaltable.Rows[0][strcolname].ToString();
                amountboundfield.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                amountboundfield.DataFormatString = "{0:n2}";                
                
                gvSecType.Columns.Add(amountboundfield);

                if (k >= agentscnt - 1)
                    k = 0;
                else
                    k = k + 1;
            }
        }
    }

    private void AddCoumnsInGridAirfare(DataTable datatable, DataTable agenttable, DataTable totaltable)
    {
        if (datatable.Columns.Count > 1 && gvCrewAirfare.Columns.Count < 6)
         {
            for (int i = 1; i < datatable.Columns.Count; i = i + 1)
            {
                BoundField amountboundfield = new BoundField();

                amountboundfield.DataField = datatable.Columns[i].ColumnName.ToString();
                amountboundfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;                
                amountboundfield.HeaderText = "ONSIGNER AMT";
                amountboundfield.DataFormatString = "{0:n2}";
                gvCrewAirfare.Columns.Add(amountboundfield);
                i = i + 1;

                amountboundfield = new BoundField();
                amountboundfield.DataField = datatable.Columns[i].ColumnName.ToString();
                amountboundfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                amountboundfield.HeaderText = "OFFSIGNER AMT";
                amountboundfield.DataFormatString = "{0:n2}";
                gvCrewAirfare.Columns.Add(amountboundfield);  
              
            }
        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvSection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }


    protected void gvSection_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvSection_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
    }

    protected void gvSection_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }
    protected void gvSection_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (gvSection.Columns.Count > 1)
            {
                int j = 0;
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                for (int i = 0; i <= gvSection.Columns.Count; i++)
                {
                    TableCell HeaderCell;
                    if (i < 2)
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "";
                        HeaderCell.ColumnSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        i = i + 2;
                    }
                    if (i >= 2 && i <= gvSection.Columns.Count)
                    {
                        HeaderCell = new TableCell();
                        string port = arrayPort[j].ToString().Trim();
                        if (j < arrayPort.Count)
                            HeaderCell.Text = port;//getFirstWord(port);
                        else
                            HeaderCell.Text = "";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = agentscnt;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        i = i + agentscnt;
                        if (agentscnt == 1)
                        {
                            if (gvSection.Columns.Count == i)
                                i = i - 1;
                            else
                                i = i + 1;
                        }
                        j++;
                    }
                }

                gvSection.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
    }
    protected void gvSecType_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (gvSecType.Columns.Count > 0)
            {
                int j = 0;
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                for (int i = 0; i <= gvSecType.Columns.Count; i++)
                {
                    TableCell HeaderCell;
                    if (i < 1)
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "";
                        HeaderCell.ColumnSpan = 1;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        i = i + 2;
                    }
                    if (i >= 1 && i <= gvSecType.Columns.Count)
                    {
                        HeaderCell = new TableCell();
                        string port = arrayPort[j].ToString().Trim();
                        if (j < arrayPort.Count)
                            HeaderCell.Text = port;//getFirstWord(port);
                        else
                            HeaderCell.Text = "";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = agentscnt;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        i = i + agentscnt;
                        if (agentscnt == 1)
                        {
                            if (gvSecType.Columns.Count == i)
                                i = i - 1;
                            else
                                i = i + 1;
                        }
                        j++;
                    }
                }

                gvSecType.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
    }
    protected void gvCrewAirfare_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (gvCrewAirfare.Columns.Count > 0)
            {
                int j = 0;
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                
                for (int i = 0; i <= gvCrewAirfare.Columns.Count; i++)
                {
                    TableCell HeaderCell;
                    if (i < 1)
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "";
                        HeaderCell.ColumnSpan = 1;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        i = i + 2;
                    }
                    if (i >= 1 && i <= gvCrewAirfare.Columns.Count)
                    {
                        HeaderCell = new TableCell();
                        string port = arrayPort[j].ToString();
                        if (j < arrayPort.Count)
                            HeaderCell.Text = port;
                        else
                            HeaderCell.Text = "";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = 2;
                        HeaderGridRow.Cells.Add(HeaderCell);

                        //i = i + colcnt;
                        //if (colcnt == 1)
                        //{
                        //    if (gvCrewAirfare.Columns.Count == i)
                        //        i = i - 1;
                        //    else
                        //        i = i + 1;
                        //}
                        i = i + 1;
                        j++;                       
                    }
                }

                gvCrewAirfare.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
      
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    private void MergeRows(GridView gridView)
    {
        for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = gridView.Rows[rowIndex];
            GridViewRow previousRow = gridView.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count - 2; i++)
            {
                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                {
                    if (i == 1)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan;
                        previousRow.Cells[i].Visible = false;
                    }

                }
            }
        }
    }
    private void GenerateUniqueData(int cellno)
    {
        //Logic for unique names

        //Step 1:
        Label tx = (Label)gvSection.Rows[0].FindControl("lblSectionTypeName");
        Label tx1 = (Label)gvSection.Rows[0].FindControl("lblSectionName");
        string initialnamevalue = gvSection.Rows[0].Cells[cellno].Text;
        if (cellno == 1)
        {
            for (int i = 1; i < gvSection.Rows.Count; i++)
            {
                if (((Label)gvSection.Rows[i].FindControl("lblSectionTypeName")).Text == tx.Text)
                {
                    ((Label)gvSection.Rows[i].FindControl("lblSectionTypeName")).Text = string.Empty;
                }
                else
                    tx = (Label)gvSection.Rows[i].FindControl("lblSectionTypeName");
            }

        }


        if (((cellno - 2) % 4) == 0)
        {
            tx = (Label)gvSection.Rows[0].FindControl("lblSectionTypeName");
            tx1 = (Label)gvSection.Rows[0].FindControl("lblSectionName");
            //Step 2:        

            for (int i = 1; i < gvSection.Rows.Count; i++)
            {

                if (gvSection.Rows[i].Cells[cellno - 1].Text == initialnamevalue && ((Label)gvSection.Rows[i].FindControl("lblName")).Text == tx.Text && ((Label)gvSection.Rows[i].FindControl("lblOrigin")).Text == tx1.Text)
                {
                    gvSection.Rows[i].Cells[cellno - 1].Text = string.Empty;
                    gvSection.Rows[i].Cells[cellno + 1].Text = string.Empty;
                    gvSection.Rows[i].Cells[cellno + 2].Text = string.Empty;
                    gvSection.Rows[i].Cells[cellno].Text = string.Empty;
                }
                else
                {
                    initialnamevalue = gvSection.Rows[i].Cells[cellno].Text;
                    tx = (Label)gvSection.Rows[i].FindControl("lblSectionTypeName");
                    tx1 = (Label)gvSection.Rows[i].FindControl("lblSectionName");
                }
            }
        }
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
    }

    
    protected void MenuAirfare_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void gvCrewAirfare_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvCrewAirfare_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }
    protected void gvCrewAirfare_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public string getFirstWord(string str)
    {
        string firstword ="";
        firstword = (str.Substring(0, str.IndexOf(" ") == -1 ? (str.Length - 1) : (str.IndexOf(" ")))) + ".";
        return firstword;
    }
}
