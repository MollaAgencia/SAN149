using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aplicacao.Models.SITE.Login;
using MollaLibrary.COMMON;
using MollaLibrary.ExtendMethods;

namespace Aplicacao.Models.COMMON.API
{
    public abstract class MetodosAPI 
    {
        private IMetodosAPI pessoa = null;
        protected MollaLibraryAPI.COMMON.EnunsAPP.TipoPessoa PRP_TipoPessoa { get; set; }
        public MetodosAPI(MollaLibraryAPI.COMMON.EnunsAPP.TipoPessoa pTipoPessoa)
        {
            PRP_TipoPessoa = pTipoPessoa;
            if (pTipoPessoa == MollaLibraryAPI.COMMON.EnunsAPP.TipoPessoa.Fisica)
            {
                pessoa = new PessoaFisica();
            }
        }

        public virtual RetornoRequisicao MTD_AceiteRegulamento(string pLogin) => pessoa.MTD_AceiteRegulamento(pLogin);

        /// <summary>
        /// Método de Cadastro e alteração de usuário | Para Pessoa fisica utilizar a classe MollaLibraryAPI.PessoaFisica.CRUD e para pessoa Juridica  MollaLibraryAPI.PessoaJuridica.CRUD
        /// </summary>
        /// <param name="cadastrais"></param>
        /// <param name="pComplemento"></param>
        /// <returns></returns>
        public virtual RetornoRequisicao MTD_CRUD(Models.SITE.Login.DadosUsuario cadastrais) => pessoa.MTD_CRUD(cadastrais);
        public virtual RetornoRequisicao MTD_AlterarSenha(string pNovaSenha) => pessoa.MTD_AlterarSenha(pNovaSenha);
        public virtual RetornoRequisicao MTD_Autenticacao(string pLogin, string pSenha, out bool pAcessoFake) => pessoa.MTD_Autenticacao(pLogin, pSenha, out pAcessoFake);
        public virtual dynamic MTD_ListarUsuarios(string pPIT) => pessoa.MTD_ListarUsuarios(pPIT);
        /// <summary>
        /// Método retorna as informações equivalentes a autenticação do usuário | MollaLibraryAPI.PessoaFisica.PessoaRetorno
        /// </summary>
        /// <param name="pLogin">Login que o usuário utiliza para se autenticar (CPF)</param>
        /// <returns>Retorna os dados | MollaLibraryAPI.PessoaFisica.PessoaRetorno</returns>
        public virtual dynamic MTD_ObterUsuario(string pLogin) => pessoa.MTD_ObterUsuario(pLogin);
        public virtual dynamic MTD_ObterUsuarioCrud(string pLogin) => pessoa.MTD_ObterUsuarioCrud(pLogin);
       
    }
}