-- FUNCTION: public.f_balances()

-- DROP FUNCTION IF EXISTS public.f_balances();

CREATE OR REPLACE FUNCTION public.f_balances(
	)
    RETURNS TABLE(id text, companyid integer, companyname text, name text, phonenumber text, total numeric) 
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
	SELECT "Id", 
		"CompanyId", 
		"CompanyName", 
		"Name", 
		"PhoneNumber", 
		"Total"
	FROM public."vBalances";
$BODY$;

ALTER FUNCTION public.f_balances()
    OWNER TO postgres;

