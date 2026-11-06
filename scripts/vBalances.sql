-- View: public.vBalances

-- DROP VIEW public."vBalances";

CREATE OR REPLACE VIEW public."vBalances"
 AS
 SELECT u."Id",
    u."CompanyId",
    c."Name" AS "CompanyName",
    u."Name",
    u."PhoneNumber",
    v."Total"
   FROM "AspNetUsers" u
     LEFT JOIN "Companies" c ON c."Id" = u."CompanyId"
     LEFT JOIN "vReceiptTotalAmounts" v ON v."Id" = u."Id"
  WHERE v."Total" IS NOT NULL AND v."Total" <> 0::numeric
  ORDER BY u."Name";

ALTER TABLE public."vBalances"
    OWNER TO postgres;
COMMENT ON VIEW public."vBalances"
    IS 'Balance de todos los recibos, muestra la diferencias entre los Recibos de ingreso y egreso, si son negativos te deben o debes plata, depende de quien es el usuario';

