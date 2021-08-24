using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;


public partial class CommonPickListLubOilAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");
        MenuAddress.MenuList = toolbarmain.Show();
        //MenuAddress.SetTrigger(pnlAddressEntry);        

        if (!IsPostBack)
        {
           
            if ((Request.QueryString["addresstype"] != null) && (Request.QueryString["addresstype"] != ""))
                ViewState["addresstype"] = "," + Request.QueryString["addresstype"].ToString() + ",";
            //else
            //    Response.Redirect("../PhoenixUnderConstruction.aspx");
            if (Request.QueryString["txtsupcode"] != null)
            {
                txtCode.Text = Request.QueryString["txtsupcode"].ToString(); 
            }         
            if (Request.QueryString["txtsupname"] != null)
            {
                txtNameSearch.Text = Request.QueryString["txtsupname"].ToString();
            }
            if ((Request.QueryString["producttype"] != null) && (Request.QueryString["productype"] != ""))
            {
                ViewState["producttype"] = "," + Request.QueryString["producttype"].ToString() + ",";

                string[] producttype = ViewState["producttype"].ToString().Split(new string[] {","}, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in producttype)
                {
                    if (item.Trim() != "")
                    {
                        //cblProductType.Items.FindByValue(item).Selected = true;
                    }
                }
            }
            if (Request.QueryString["framename"] != null)
            {
               ViewState["framename"] = Request.QueryString["framename"].ToString();
            }

            if ((Request.QueryString["emailid"] != null) && (Request.QueryString["emailid"] != ""))
                ViewState["emailid"] = Request.QueryString["emailid"].ToString();


            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string addresstype = null;
            string producttype = null;
            if (ViewState["addresstype"] != null)
                addresstype = ViewState["addresstype"].ToString();
            if (ViewState["producttype"] != null)
                producttype = ViewState["producttype"].ToString();
            else
                producttype = "";

            string emailid = null;
            if (ViewState["emailid"] != null)
                emailid = ViewState["emailid"].ToString();

                ds = PhoenixCommonRegisters.LubOilAddressSearch(sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);
            

                gvAddress.DataSource = ds;
            gvAddress.VirtualItemCount = iRowCount;
           ViewState["ROWCOUNT"] = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuAddress_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {

                ViewState["PAGENUMBER"] = 1;

                NameValueCollection addresscriteria = new NameValueCollection();

                addresscriteria.Clear();
                addresscriteria.Add("txtNameSearch", txtNameSearch.Text);
                addresscriteria.Add("txtCode", txtCode.Text);
                addresscriteria.Add("txtCountryNameSearch", txtCountryNameSearch.Text);

                Filter.CurrentCommonAddressFilterCriteria = addresscriteria;

                StringBuilder strproducttype = new StringBuilder();

                //foreach (ListItem item in cblProductType.Items)
                {
                   
                }
                if (strproducttype.Length > 1)
                {
                    strproducttype.Remove(strproducttype.Length - 1, 1);
                }

                ViewState["producttype"] = strproducttype.ToString();

                BindData();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page" || e.CommandName == "ChangePageSize")
            return;

        string Script = "";
        RadGrid _gridView = (RadGrid)sender;
        NameValueCollection nvc;

        if (e.CommandName.ToUpper().Equals("RELATION"))
        {
            RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");
            Response.Redirect("../Common/CommonPickListAddressRelation.aspx?addresscode=" + lblAddressCode.Text + "&addressscreen=ADDRESS&addresstype=" + Request.QueryString["addresstype"]);
        }

        if (Request.QueryString["mode"] == "custom")
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                if (ViewState["framename"] != null)
                    Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = new NameValueCollection();
            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            nvc.Add(lblCode.ID, lblCode.Text);
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
            nvc.Add(lb.ID, lb.Text.ToString());
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
            nvc.Add(lbl.ID, lbl.Text.ToString());

        }
        else if (Request.QueryString["emailyn"] == "1")
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblCode.Text);

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lbl.Text);

            RadLabel lblemail = (RadLabel)e.Item.FindControl("lblEmail");
            nvc.Set(nvc.GetKey(4), lblemail.Text);
        }
        else
        {
            if (Request.QueryString["ignoreiframe"] != null)
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                Script += "</script>" + "\n";
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (ViewState["framename"] != null)
                    Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            nvc = Filter.CurrentPickListSelection;

            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            nvc.Set(nvc.GetKey(1), lblCode.Text);

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkAddressName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblAddressCode");
            nvc.Set(nvc.GetKey(3), lbl.Text);

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);

        }

    
        //BindData();
    }

    protected void gvAddress_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvAddress_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        if(e.Item is GridEditableItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
              //&& !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblEmail1");
                RadLabel lblAddressCode = (RadLabel)e.Item.FindControl("lblAddressCode");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucEmailTT");

                HtmlImage img = (HtmlImage)e.Item.FindControl("imgEmail");
                if (img != null)
                    img.Attributes.Add("onclick", "javascript:Openpopup('codehelp2','', '../Registers/RegistersAddressEmail.aspx?addresscode=" + lblAddressCode.Text + "', 'medium')");                

                if (lbtn != null && uct != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
                lbtn = (RadLabel)e.Item.FindControl("lblServices");
                uct = (UserControlToolTip)e.Item.FindControl("ucServicesTT");

                if (lbtn != null && uct != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
        //if (e.Row.RowType == DataControlRowType.Header)
        if(e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvAddress_SortCommand(object sender, GridSortCommandEventArgs e)
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

  

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
    }
  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        //gvAddress.SelectedIndex = -1;
        //gvAddress.EditIndex = -1;

        BindData();
    }

    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
