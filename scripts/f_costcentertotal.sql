-- FUNCTION: public.f_costcentertotal(timestamp without time zone, timestamp without time zone)

-- DROP FUNCTION IF EXISTS public.f_costcentertotal(timestamp without time zone, timestamp without time zone);

CREATE OR REPLACE FUNCTION public.f_costcentertotal(
	p_date_from timestamp without time zone,
	p_date_to timestamp without time zone)
    RETURNS TABLE(costcenterid integer, costcentername text, totalcount bigint, subtotal numeric) 
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
    SELECT
        r."CostCenterId",
        r."CostCenterName",
        COUNT(r."Id"),
        SUM(r."Amount")
    FROM public."vReceipts" r
    WHERE r."CompanyId" IS NOT NULL
      AND r."Amount" < 0
      AND r."Date" BETWEEN p_date_from AND p_date_to
    GROUP BY r."CostCenterId", r."CostCenterName"

    UNION ALL

    SELECT
        r."CostCenterId",
        r."CostCenterName",
        COUNT(r."Id"),
        SUM(r."Amount")
    FROM public."vReceipts" r
    WHERE r."CompanyId" IS NOT NULL
      AND r."Amount" > 0
      AND r."Date" BETWEEN p_date_from AND p_date_to
    GROUP BY r."CostCenterId", r."CostCenterName";
$BODY$;

ALTER FUNCTION public.f_costcentertotal(timestamp without time zone, timestamp without time zone)
    OWNER TO postgres;

