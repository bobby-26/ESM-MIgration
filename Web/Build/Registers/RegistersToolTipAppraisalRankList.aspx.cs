using System;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;

public partial class RegistersToolTipAppraisalRankList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["rankcategoryid"] = "";
            ViewState["rankcategorylist"] = "";
            if (Request.QueryString["rankcategoryid"] != null && Request.QueryString["rankcategoryid"] != null)
                ViewState["rankcategoryid"] = Request.QueryString["rankcategoryid"].ToString();

            if (Request.QueryString["rankcategorylist"] != null && Request.QueryString["rankcategorylist"] != null)
                ViewState["rankcategorylist"] = Request.QueryString["rankcategorylist"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDRANKCODE", "FLDRANKNAME" };
        string[] alCaptions = { "Rank Code", "Rank" };

        DataTable dt = new DataTable();
        if (ViewState["rankcategorylist"].ToString() != null && ViewState["rankcategorylist"].ToString() != "")
        {
            dt = PhoenixRegistersAppraisalConfig.AppraisalRankListByRankGroupCategory(General.GetNullableInteger(ViewState["rankcategoryid"].ToString())
                , ViewState["rankcategorylist"].ToString());
        }
        //else
        //{
        //    dt = PhoenixRegistersAppraisalConfig.AppraisalRankListByCategory(General.GetNullableInteger(ViewState["rankcategoryid"].ToString()));
        //}
        lblGrid.Text = General.ShowGrid(dt, alColumns, alCaptions);
    }
}
