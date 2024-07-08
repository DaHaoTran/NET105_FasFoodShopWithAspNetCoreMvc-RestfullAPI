
var app = angular.module("myapp", []);
app.controller("navCtrl", function ($rootScope) {
    $rootScope.cart = []
    $rootScope.incart = 0;
    $rootScope.city = "Hồ Chí Minh";
    $rootScope.district = [
        { name: "Quận 1" },
        { name: "Quận 2" },
        { name: "Quận 3" },
        { name: "Quận 4" },
        { name: "Quận 5" },
        { name: "Quận 6" },
        { name: "Quận 7" },
        { name: "Quận 8" },
        { name: "Quận 9" },
        { name: "Quận 10" },
        { name: "Quận 11" },
        { name: "Quận 12" },
        { name: "Quận Bình Tân" },
        { name: "Quận Bình Thạnh" },
        { name: "Quận Gò Vấp" },
        { name: "Quận Phú Nhuận" },
        { name: "Quận Tân Bình" },
        { name: "Quận Tân Phú" },
        { name: "Quận Thủ Đức" },
        { name: "Huyện Bình Chánh" },
        { name: "Huyện Cần Giờ" },
        { name: "Huyện Củ Chi" },
        { name: "Huyện Hóc Môn" },
        { name: "Huyện Nhà Bè" }
    ];
})
app.controller("bodyCtrl", function ($rootScope, $scope) {
    $scope.products = [];
    $scope.quantityset = 1;

    $scope.Addtocart = function (code) {
        if ($rootScope.cart.findIndex(x => x.code == code) === -1)
        {
            var getter = $scope.products.find(x => x.FoodCode == code);
            var add = { code: getter.FoodCode, image: getter.Image, name: getter.FoodName, price: getter.CurrentPrice, quantity: $scope.quantityset, total: parseInt(getter.CurrentPrice) * parseInt($scope.quantityset) };
            $rootScope.cart.push(add);
        }
        else
        {
            var index = $rootScope.cart.findIndex(x => x.code == code);
            $rootScope.cart[index].quantity += $scope.quantityset;
            $rootScope.cart[index].total = $rootScope.cart[index].price * $rootScope.cart[index].quantity;
        };
        $rootScope.incart = $rootScope.cart.length;
    };

    var left = 0;

    $scope.GetFood = function (food, code) {
        var getter = food.find(x => x.FoodCode == code)
        $scope.codeset = getter.FoodCode
        $scope.imageset = getter.Image
        $scope.nameset = getter.FoodName
        $scope.priceset = getter.CurrentPrice
        left = getter.Left
        if (left == parseInt(document.querySelector("#number").value)) {
            document.querySelector("#limitmess").innerHTML = "Đã đạt giới hạn tối đa"
        }
    }

    $scope.Removeitem = function (index) {
        $rootScope.cart.splice(index, 1);
        $rootScope.incart = $rootScope.cart.length;
    }

    $scope.decreaseValue = function () {
        if ($scope.quantityset - 1 == 0) {
            $scope.quantityset--;
        }
    }

    $scope.increaseValue = function () {
        if ($scope.quantityset + 1 > left) {
            document.querySelector("#limitmess").innerHTML = "Đã đạt giới hạn tối đa"
        } else {
            $scope.quantityset++;
        }
    }
});