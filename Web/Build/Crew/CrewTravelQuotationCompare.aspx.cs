using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class CrewTravelQuotationCompare : PhoenixBasePage
{
    ArrayList arrayUser = new ArrayList();
    ArrayList arrayid = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                 ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                    ViewState["AGENTS"] = Request.QueryString["AGENTS"].ToString(); 
                 
                }
                else
                {
                    ViewState["TRAVELID"] = "0";
                    ViewState["selectagents"] = "";
                }
                if (Request.QueryString["VIEWONLY"] == null)
                {
                    PhoenixToolbar toolbargrid = new PhoenixToolbar();                    
                    toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp2','Quotation','" + Session["sitepath"] + "/Crew/CrewTravelQuotationCompare.aspx?TRAVELID=" + Request.QueryString["TRAVELID"] + "&AGENTS=" + ViewState["AGENTS"].ToString() + "&VIEWONLY=1');return false;", "View", "<i class=\"fas fa-glasses\"></i>", "");
                    MenuQuotationCompare.MenuList = toolbargrid.Show();
                }               
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuQuotationCompare_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {

        }
    }

      
    private void BindData()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTravelQuote.QuotationCompare(General.GetNullableGuid(ViewState["TRAVELID"].ToString()), ViewState["AGENTS"].ToString());
        
        arrayUser.Clear();
       
        foreach (DataRow dr in ds.Tables[1].Rows)
        {
            GridColumnGroup colgroup = new GridColumnGroup();
            colgroup.HeaderText = dr["FLDNAME"].ToString();
            colgroup.Name = dr["FLDAGENTID"].ToString();            
            gvAgent.MasterTableView.ColumnGroups.Add(colgroup);
            //agentList.Add(dr["FLDAGENTID"].ToString(), dr["FLDNAME"].ToString());
            arrayUser.Add(dr["FLDNAME"].ToString());
            arrayid.Add(dr["FLDAGENTID"].ToString());  
        }
       if (ds.Tables[0].Rows.Count > 0)
       {
           AddCoumnsInGrid(ds.Tables[0], ds.Tables[1]);
           gvAgent.DataSource = ds;         
           if (gvAgent.Columns.Count > 5)
           {
               CheckLowestAmount();
           }
       }
       else
       {
            gvAgent.DataSource = "";
       }
    }

    private void CheckLowestAmount()
    {
        if (gvAgent.Columns.Count > 5)
        {
            foreach (GridViewRow gvr in gvAgent.MasterTableView.Items)
            {
                decimal lowamount = CheckDecimal(gvr.Cells[8].Text);
                int k = 8;
                for (int i = 8; i < gvAgent.Columns.Count; i +=4)
                {
                    if (lowamount > CheckDecimal(gvr.Cells[i].Text) && (CheckDecimal(gvr.Cells[i].Text) > 0))
                    {
                        lowamount = CheckDecimal(gvr.Cells[i].Text);
                        k = i;
                    }
                }
                gvr.Cells[4].Text = lowamount.ToString();
                gvr.Cells[k].ForeColor = System.Drawing.Color.Chocolate;
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

    private void AddCoumnsInGrid(DataTable datatable,DataTable agenttable)
    {
        if (datatable.Columns.Count > 1 && gvAgent.Columns.Count < 6)
        {
            for (int i = 3,a=0; i < datatable.Columns.Count-2; i += 4,a++)
            {
                GridBoundColumn quote = new GridBoundColumn();
                quote.DataField = datatable.Columns[i].ColumnName;
                quote.ColumnGroupName = agenttable.Rows[a]["FLDAGENTID"].ToString();
                quote.HeaderText = "Quote";
                gvAgent.Columns.Add(quote);

                GridBoundColumn quotenumber = new GridBoundColumn();
                quotenumber.DataField = datatable.Columns[i + 1].ColumnName;
                quotenumber.ItemStyle.CssClass = "txtNumber";
                quotenumber.HeaderText = "Stop";
                quotenumber.DataFormatString = "{0:n2}";
                quotenumber.ColumnGroupName = agenttable.Rows[a]["FLDAGENTID"].ToString();
                gvAgent.Columns.Add(quotenumber);

                GridBoundColumn amountboundfield = new GridBoundColumn();
                amountboundfield.DataField = datatable.Columns[i + 2].ColumnName;
                amountboundfield.ItemStyle.CssClass = "txtNumber";
                amountboundfield.HeaderText = "Amount";
                amountboundfield.DataFormatString = "{0:n2}";
                amountboundfield.ColumnGroupName = agenttable.Rows[a]["FLDAGENTID"].ToString();
                gvAgent.Columns.Add(amountboundfield);

                GridBoundColumn amountusd = new GridBoundColumn();
                amountusd.DataField = datatable.Columns[i + 3].ColumnName;
                amountusd.ItemStyle.CssClass = "txtNumber";
                amountusd.HeaderText = "Amount (USD)";
                amountusd.DataFormatString = "{0:n2}";
                amountusd.ColumnGroupName = agenttable.Rows[a]["FLDAGENTID"].ToString();
                gvAgent.Columns.Add(amountusd);
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvAgent.Rebind();
    }
    protected void btnViewAgenterDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Crew/CrewTravelQuotationAgent.aspx");  
    }
    
    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
