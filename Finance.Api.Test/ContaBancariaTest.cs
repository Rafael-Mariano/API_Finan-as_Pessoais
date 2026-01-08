using Xunit;
using Finance.Api.Domain;

namespace Finance.Api.Test
{
    public class ContaBancariaTest
    {
        [Fact]
        public void NovaConta_RetornaValorZerado()
        {
            var conta = new ContaBancaria("Conta Teste");

            Assert.Equal(0m, conta.Saldo);
        }

        [Fact]
        public void Depositar_AdicionarValor_RetornaSaldoAtualizado()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Depositar(100);

            Assert.Equal(100, conta.Saldo);
        }

        [Fact]
        public void Sacar_SubtrairValor_RetornaSaldoAtualizado()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Sacar(20);

            Assert.Equal(-20, conta.Saldo);
        }

        [Fact]
        public void Sacar_ValorMaiorQueSaldo_LancarExcessao()
        {
            var conta = new ContaBancaria("Conta Teste");

            conta.Depositar(100);
            conta.Sacar(100);

            Assert.Throws<InvalidOperationException>(() => conta.Sacar(100m));
        }
    }
}